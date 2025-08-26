const CACHE = "app-v1";
const ASSETS = [
    "/Auth/Login",                     // start_url
    "/css/site.css",         // kendi css
    "/js/jquery-3.7.1.min.js",
    "/js/bootstrap/bootstrap.min.js",
    "/js/intersections.js",
    "/offline.html"          // offline fallback (3. adımda ekleyeceğiz)
];

self.addEventListener("install", (e) => {
    e.waitUntil(caches.open(CACHE).then(c => c.addAll(ASSETS)));
    self.skipWaiting();
});

self.addEventListener("activate", (e) => {
    e.waitUntil(
        caches.keys().then(keys =>
            Promise.all(keys.filter(k => k !== CACHE).map(k => caches.delete(k)))
        )
    );
    self.clients.claim();
});

// App Shell: önce cache, yoksa ağ; navigasyonlar için offline fallback
self.addEventListener("fetch", (e) => {
    const req = e.request;

    // Sayfa gezintileri (document) için offline.html düş
    if (req.mode === "navigate") {
        e.respondWith(
            fetch(req).catch(() => caches.match("/offline.html"))
        );
        return;
    }

    // Diğerleri: cache-then-network
    e.respondWith(
        caches.match(req).then(cached => cached || fetch(req))
    );
});