using BusinessLayer.DTOs;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Utils
{
    public static class ChangeDiffBuilder
    {
        static readonly string[] ScalarIgnore =
        {
            nameof(IntersectionUpdateDto.IntersectionID)
        };

        public static void DiffScalars(IntersectionUpdateDto cur, IntersectionUpdateDto prop, List<FieldDiff> outList)
            => DiffByReflection(cur, prop, outList, prefix: "", ScalarIgnore);
        public static void DiffSubstructure(SubstructureDto? cur, SubstructureDto? prop, List<FieldDiff> outList)
            => DiffByReflection(cur, prop, outList, prefix: "Substructure");

        static void DiffByReflection(object? cur, object? prop, List<FieldDiff> outList, string prefix, params string[] ignore)
        {
            if (cur == null && prop == null) return;
            if (cur == null || prop == null)
            {
                outList.Add(new FieldDiff
                {
                    Path = prefix,
                    OldValue = cur == null ? null : Mini(cur),
                    NewValue = prop == null ? null : Mini(prop)
                });
                return;
            }

            var t = cur.GetType();
            foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var path = string.IsNullOrEmpty(prefix) ? p.Name : $"{prefix}.{p.Name}";
                if (ignore.Contains(p.Name)) continue;

                var a = p.GetValue(cur);
                var b = p.GetValue(prop);

                if (IsScalar(p.PropertyType))
                {
                    if (!EqualsAsText(a, b))
                    {
                        outList.Add(new FieldDiff { Path = path, OldValue = ToText(a), NewValue = ToText(b) });
                    }
                }
            }
        }

        static bool IsScalar(Type t)
        {
            t = Nullable.GetUnderlyingType(t) ?? t;
            return t.IsPrimitive || t.IsEnum || t == typeof(string) || t == typeof(decimal) || t == typeof(DateTime) || t == typeof(double);
        }

        static string? ToText(object? o) => o switch
        {
            null => null,
            bool b => b ? "true" : "false",
            DateTime d => d.ToString("yyyy-MM-dd HH:mm"),
            _ => o.ToString()
        };
        static bool EqualsAsText(object? a, object? b) => ToText(a) == ToText(b);
        static string Mini(object o) => System.Text.Json.JsonSerializer.Serialize(o);

        public static void DiffLocations(ICollection<Location> current, IList<LocationDto> proposed, List<LocationChange> outList)
        {
            var curById = current.ToDictionary(x => x.LocationID, x => x);
            var propById = proposed.Where(x => x.LocationID.HasValue).ToDictionary(x => x.LocationID!.Value, x => x);

            // Removed
            foreach (var cur in current)
            {
                if (!propById.ContainsKey(cur.LocationID))
                {
                    outList.Add(new LocationChange { ChangeType = "Removed", LocationID = cur.LocationID });
                }
            }
            // Added / Updated
            foreach (var p in proposed)
            {
                if (!p.LocationID.HasValue)
                {
                    outList.Add(new LocationChange
                    {
                        ChangeType = "Added",
                        LocationID = null,
                        FieldDiffs = new List<FieldDiff>
                    {
                        new FieldDiff { Path = "City",      OldValue = null, NewValue = p.City },
                        new FieldDiff { Path = "Latitude",  OldValue = null, NewValue = p.Latitude.ToString() },
                        new FieldDiff { Path = "Longitude", OldValue = null, NewValue = p.Longitude.ToString() },
                    }
                    });
                }
                else
                {
                    if (curById.TryGetValue(p.LocationID.Value, out var cur))
                    {
                        var diffs = new List<FieldDiff>();
                        if (!EqualsAsText(cur.City, p.City)) diffs.Add(new FieldDiff { Path = "City", OldValue = cur.City, NewValue = p.City });
                        if (!EqualsAsText(cur.Latitude, p.Latitude)) diffs.Add(new FieldDiff { Path = "Latitude", OldValue = cur.Latitude.ToString(), NewValue = p.Latitude.ToString() });
                        if (!EqualsAsText(cur.Longitude, p.Longitude)) diffs.Add(new FieldDiff { Path = "Longitude", OldValue = cur.Longitude.ToString(), NewValue = p.Longitude.ToString() });
                        if (diffs.Count > 0)
                            outList.Add(new LocationChange { ChangeType = "Updated", LocationID = cur.LocationID, FieldDiffs = diffs });
                    }
                    else
                    {
                        // güvenli: listedeyse ama DB'de yoksa "Added" say
                        outList.Add(new LocationChange { ChangeType = "Added", LocationID = p.LocationID });
                    }
                }
            }
        }

        public static void DiffImages(ICollection<IntersectionImage> current, ImageChangeDto? proposed, List<ImageChange> outList)
        {
            if (proposed == null) return;
            // Removed
            if (proposed.DeleteIds != null && proposed.DeleteIds.Count > 0)
            {
                foreach (var id in proposed.DeleteIds)
                {
                    var cur = current.FirstOrDefault(x => x.ImageID == id);
                    outList.Add(new ImageChange { ChangeType = "Removed", ImageId = id, Path = cur?.ImagePath });
                }
            }
            // Added (staging path)
            if (proposed.AddStagingPaths != null && proposed.AddStagingPaths.Count > 0)
            {
                foreach (var sp in proposed.AddStagingPaths)
                    outList.Add(new ImageChange { ChangeType = "Added", Path = sp });
            }
        }

    }
}
