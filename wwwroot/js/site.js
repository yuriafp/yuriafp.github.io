(function () {
    var CYCLE = 9000;
    var FLOOR = 0.15;
    var FRAME_MS = 33;

    var canvas, ctx, width, height, levels, raf, lastPaint;
    var reduced = window.matchMedia('(prefers-reduced-motion: reduce)');

    function buildLevels(w, h) {
        var built = [];

        function walk(x, y, bw, bh, depth, level) {
            (built[level] || (built[level] = [])).push([x, y, bw, bh]);
            if (depth === 0 || bw < 32) return;

            var hw = bw / 2, hh = bh / 2;
            walk(x, y, hw, hh, depth - 1, level + 1);
            walk(x + hw, y, hw, hh, depth - 1, level + 1);
            walk(x + hw, y + hh, hw, hh, depth - 1, level + 1);
        }

        var hw = w / 2, hh = h / 2;
        walk(0, 0, hw, hh, 4, 0);
        walk(hw, 0, hw, hh, 4, 0);
        walk(0, hh, hw, hh, 4, 0);
        walk(hw, hh, hw, hh, 4, 0);

        return built;
    }

    function alphaFor(level, now) {
        var phase = (now % CYCLE) / CYCLE;
        var peak = level / levels.length;
        var d = Math.abs(phase - peak);
        if (d > 0.5) d = 1 - d;
        return FLOOR + (1 - FLOOR) * Math.exp(-(d * d) / 0.016);
    }

    function paint(now) {
        ctx.clearRect(0, 0, width, height);

        for (var i = 0; i < levels.length; i++) {
            ctx.globalAlpha = now === null ? 1 : alphaFor(i, now);

            var rects = levels[i];
            ctx.beginPath();
            for (var j = 0; j < rects.length; j++) {
                ctx.rect(rects[j][0], rects[j][1], rects[j][2], rects[j][3]);
            }
            ctx.stroke();
        }
    }

    function frame(now) {
        if (now - lastPaint >= FRAME_MS) {
            lastPaint = now;
            paint(now);
        }
        raf = requestAnimationFrame(frame);
    }

    function start() {
        if (raf || reduced.matches) return;
        lastPaint = 0;
        raf = requestAnimationFrame(frame);
    }

    function stop() {
        if (!raf) return;
        cancelAnimationFrame(raf);
        raf = 0;
    }

    function setup() {
        canvas = canvas || document.getElementById('backdrop');
        if (!canvas) return;

        var ratio = window.devicePixelRatio || 1;
        width = window.innerWidth;
        height = window.innerHeight;

        canvas.width = Math.round(width * ratio);
        canvas.height = Math.round(height * ratio);

        ctx = canvas.getContext('2d');
        ctx.scale(ratio, ratio);
        ctx.strokeStyle = '#22c55e';
        ctx.lineWidth = 1;

        levels = buildLevels(width, height);

        paint(reduced.matches ? null : 0);
        requestAnimationFrame(function () { canvas.classList.add('is-ready'); });
    }

    var resizeTimer;
    window.addEventListener('resize', function () {
        clearTimeout(resizeTimer);
        resizeTimer = setTimeout(function () {
            stop();
            setup();
            start();
        }, 200);
    });

    // Sem isto o loop continua rodando com a aba em segundo plano, gastando
    // bateria para desenhar o que ninguém está vendo.
    document.addEventListener('visibilitychange', function () {
        if (document.hidden) stop(); else start();
    });

    reduced.addEventListener('change', function () {
        stop();
        paint(reduced.matches ? null : 0);
        start();
    });

    setup();
    start();

    window.site = {
        getLang: function (key) {
            try { return localStorage.getItem(key); } catch { return null; }
        },
        setLang: function (key, value) {
            try { localStorage.setItem(key, value); } catch { /* ignorado */ }
        },
        browserLang: function () {
            return navigator.language || 'en';
        },
        setHtmlLang: function (value) {
            document.documentElement.setAttribute('lang', value);
        }
    };
})();
