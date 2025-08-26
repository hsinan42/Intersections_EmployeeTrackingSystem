
document.addEventListener('DOMContentLoaded', initMap);

function initMap() {

    let map, ib, currentInfoBoxMarkerId = null;

    if (!window.google || !google.maps) { console.error('Google Maps yüklenmedi'); return; }
    if (typeof window.InfoBox !== 'function') { console.error('InfoBox yüklenmedi'); return; }

    const defaultCenter = { lat: 39.925533, lng: 32.866287 };
    map = new google.maps.Map(document.getElementById('map'), {
        center: defaultCenter, zoom: 6,
        mapId: "dc3581395fd68c25176f4d5b",
        gestureHandling: "greedy"
    });

    ib = new InfoBox({
        content: "",
        pixelOffset: new google.maps.Size(0, -40),
        closeBoxURL: "",
        boxStyle: { background: "transparent", opacity: 1 }
    });

    google.maps.event.addListener(map, "click", () => {
        if (ib && typeof ib.close === "function") ib.close();
        currentInfoBoxMarkerId = null;
    });

    const iconMap = {
        U3: "icon_3U_32.png",
        U3withLoop: "icon_3UL_32.png",
        U3PedPress: "icon_3UP_32.png",

        U6: "icon_6U_32.png",
        U6withLoop: "icon_6UL_32.png",
        U6Adaptive: "icon_6UA_32.png",
        U6Hybrid: "icon_6UH_32.png",
        U6PedPress: "icon_6UP_32.png",

        YU3: "icon_Y3_32.png",
        YU3withLoop: "icon_Y3L_32.png",
        YU3Adaptive: "icon_Y3A_32.png",
        YU3Hybrid: "icon_Y3H_32.png",
        YU3PedPress: "icon_Y3P_32.png"
    };

    function CustomMarker(position, map, kkcid, id, title, city, deviceType) {
        this.position = position;
        this.kkcid = kkcid;
        this.id = id;
        this.title = title;
        this.city = city;
        this.deviceType = deviceType;
        this.setMap(map);
    }
    CustomMarker.prototype = Object.create(google.maps.OverlayView.prototype);
    CustomMarker.prototype.constructor = CustomMarker;

    CustomMarker.prototype.onAdd = function () {
        const div = document.createElement("div");
        const iconFile = iconMap[this.deviceType] || "traffic-light_1414624.png";
        div.style.position = "absolute";
        div.style.cursor = "pointer";
        div.style.transform = "translate(-50%, -100%)";
        div.innerHTML = `
          <div style="text-align:center; white-space:nowrap;">
            <img src="/uploads/Icons/${iconFile}" style="width:32px;height:32px;">
            <div style="background:#000;color:#fff;border-radius:5px;font-size:12px;margin-top:4px;padding:2px 6px;">
              ${this.kkcid}
            </div>
          </div>`;

        div.addEventListener("click", (e) => {
            e.stopPropagation();
            if (currentInfoBoxMarkerId === this.id) {
                if (ib && typeof ib.close === "function") ib.close();
                currentInfoBoxMarkerId = null;
                return;
            }
            const html = `
            <div style="background:#000;color:#fff;padding:12px 16px;border-radius:8px;
                        font-size:14px;max-width:240px;box-shadow:0 8px 18px rgba(0,0,0,.35);">
              <p style="margin:0;font-weight:600;">${this.title}</p>
              <p style="margin:6px 0 8px;">${this.city}</p>
              <a href="/Intersections/Details/${this.id}" target="_blank" style="color:#ffd54f;">Detay Sayfası</a>
            </div>`;
            ib.setOptions({
                closeBoxURL: "https://maps.gstatic.com/intl/en_us/mapfiles/close.gif"
            });
            ib.setContent(html);
            ib.setPosition(this.position);
            ib.open(map);
            currentInfoBoxMarkerId = this.id;
        });

        div.addEventListener("dblclick", () => {
            window.location.href = `/Intersections/Details/${this.id}`;
        });

        this.div = div;
        this.getPanes().overlayMouseTarget.appendChild(div);
    };

    CustomMarker.prototype.draw = function () {
        const p = this.getProjection().fromLatLngToDivPixel(this.position);
        this.div.style.left = p.x + "px";
        this.div.style.top = p.y + "px";
    };

    CustomMarker.prototype.onRemove = function () {
        if (this.div && this.div.parentNode) this.div.parentNode.removeChild(this.div);
        this.div = null;
    };

    fetch('/Home/GetAllLocations')
        .then(r => r.json())
        .then(data => {
            const bounds = new google.maps.LatLngBounds();
            (data || []).forEach(loc => {
                if (!loc.lat || !loc.lng) return;
                const pos = { lat: +loc.lat, lng: +loc.lng };
                new CustomMarker(pos, map, loc.kkcid, loc.id, loc.title, loc.city, loc.deviceType);
                bounds.extend(pos);
            });
            if (data && data.length) map.fitBounds(bounds);
        })
        .catch(err => console.error("Veriler alınırken hata:", err));
}