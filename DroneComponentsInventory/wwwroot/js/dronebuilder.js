document.addEventListener('DOMContentLoaded', function () {
    // Pievieno klasi pilna platuma izkārtojumam
    document.body.classList.add('builder-page');

    const selects = document.querySelectorAll('.category-select');
    const btnSave = document.getElementById('btn-save');
    const resultsDiv = document.getElementById('compatibility-results');
    const summaryDiv = document.getElementById('build-summary');
    const saveResult = document.getElementById('save-result');

    function getVal(id) {
        const el = document.getElementById(id);
        return el && el.value ? parseInt(el.value) : null;
    }

    function getSelection() {
        return {
            frameId: getVal('select-frame'),
            motorId: getVal('select-motor'),
            propellerId: getVal('select-propeller'),
            escId: getVal('select-esc'),
            batteryId: getVal('select-battery'),
            fcId: getVal('select-fc'),
            cameraId: getVal('select-camera'),
            vtxId: getVal('select-vtx'),
            videoAntennaId: getVal('select-videoantenna'),
            receiverId: getVal('select-receiver'),
            receiverAntennaId: getVal('select-receiverantenna'),
            radioControllerId: getVal('select-radiocontroller'),
            fpvGogglesId: getVal('select-fpvgoggles')
        };
    }

    function getSelectedText(id) {
        const el = document.getElementById(id);
        if (!el || !el.value) return null;
        return el.options[el.selectedIndex].text;
    }

    function updateSummary() {
        const categories = [
            { id: 'select-frame', label: 'Rāmis' },
            { id: 'select-motor', label: 'Motori (x4)' },
            { id: 'select-propeller', label: 'Propelleri (x4)' },
            { id: 'select-esc', label: 'Elektronisko ātruma regulators' },
            { id: 'select-battery', label: 'Akumulators' },
            { id: 'select-fc', label: 'Lidojuma kontrolieris' },
            { id: 'select-camera', label: 'Pirmās personas skata kamera' },
            { id: 'select-vtx', label: 'Video raidītājs' },
            { id: 'select-videoantenna', label: 'Video raidītāja antena' },
            { id: 'select-receiver', label: 'Radio uztvērējs' },
            { id: 'select-receiverantenna', label: 'Radio uztvērēja antena' },
            { id: 'select-radiocontroller', label: 'Radio kontrolieris' },
            { id: 'select-fpvgoggles', label: 'Pirmās personas skata brilles' }
        ];

        let hasSelection = false;
        let html = '<h6 style="margin-bottom:0.75rem;font-weight:600;">Izvēlētās komponentes</h6>';

        categories.forEach(function (cat) {
            const text = getSelectedText(cat.id);
            if (text) {
                hasSelection = true;
                html += '<div class="summary-card">' +
                    '<div class="label">' + cat.label + '</div>' +
                    '<div class="value">' + text + '</div>' +
                    '</div>';
            }
        });

        if (!hasSelection) {
            html = '<p class="placeholder-text">Izvēlieties komponenti</p>';
        }

        summaryDiv.innerHTML = html;
    }

    async function checkCompatibility() {
        const selection = getSelection();
        const hasAny = Object.values(selection).some(function (v) { return v !== null; });

        if (!hasAny) {
            resultsDiv.innerHTML = '<p class="placeholder-text">Izvēlies vismaz vienu komponenti</p>';
            return;
        }

        resultsDiv.innerHTML = '<p class="placeholder-text">Pārbauda...</p>';

        try {
            const response = await fetch('/DroneBuilder/CheckCompatibility', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(selection)
            });
            const results = await response.json();
            displayResults(results);
        } catch (err) {
            resultsDiv.innerHTML = '<p style="color:#dc3545;font-size:0.8rem;">Pārbaude neizdevās: ' + err.message + '</p>';
        }
    }

    function displayResults(results) {
        var statusOrder = { critical: 0, warning: 1, ok: 2, unchecked: 3 };

        results.sort(function (a, b) {
            var statusLower_a = (a.status || '').toLowerCase();
            var statusLower_b = (b.status || '').toLowerCase();
            var sa = statusOrder[statusLower_a] !== undefined ? statusOrder[statusLower_a] : 3;
            var sb = statusOrder[statusLower_b] !== undefined ? statusOrder[statusLower_b] : 3;
            if (sa !== sb) return sa - sb;
            return b.priority - a.priority;
        });

        // Saskaita statusus
        var counts = { ok: 0, critical: 0, warning: 0, unchecked: 0 };
        results.forEach(function (r) {
            var statusLower = (r.status || '').toLowerCase();
            counts[statusLower] = (counts[statusLower] || 0) + 1;
        });

        var html = '<div class="status-summary">' +
            '<span class="status-count status-count-ok">Labi: ' + counts.ok + '</span>' +
            '<span class="status-count status-count-critical">Kritiski: ' + counts.critical + '</span>' +
            '<span class="status-count status-count-warning">Brīdinājums: ' + counts.warning + '</span>' +
            '<span class="status-count status-count-unchecked">Nav informācijas: ' + counts.unchecked + '</span>' +
            '</div>';

        results.forEach(function (r) {
            var statusLower = (r.status || '').toLowerCase();
            var label = 'Nav izvēlēta';
            var cls = 'check-unchecked';
            if (statusLower === 'ok') { label = 'Labi'; cls = 'check-ok'; }
            else if (statusLower === 'critical') { label = 'Neder'; cls = 'check-critical'; }
            else if (statusLower === 'warning') { label = 'Brīdinājums'; cls = 'check-warning'; }

            html += '<div class="check-item ' + cls + '">' +
                '<strong>[' + label + '] #' + r.checkNumber + ' ' + r.name + '</strong>' +
                '<p>' + r.message + '</p>' +
                '</div>';
        });

        resultsDiv.innerHTML = html;
    }

    // Izceļ izvēlētās izvēlnes un veic pārbaudi reāllaikā
    selects.forEach(function (s) {
        s.addEventListener('change', function () {
            if (this.value) {
                this.classList.add('has-selection');
            } else {
                this.classList.remove('has-selection');
            }
            updateSummary();
            checkCompatibility();
        });
    });

    btnSave.addEventListener('click', async function () {
        var name = document.getElementById('build-name').value.trim();
        if (!name) {
            saveResult.innerHTML = '<p style="color:#dc3545;font-size:0.8rem;">Lūdzu, ievadiet nosaukumu!</p>';
            return;
        }

        var selection = getSelection();
        selection.name = name;

        try {
            var response = await fetch('/DroneBuilder/SaveBuild', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(selection)
            });
            var result = await response.json();

            if (result.success) {
                saveResult.innerHTML = '<p style="color:#198754;font-size:0.8rem;">Saglabāts! (ID: ' + result.buildId + ')</p>';
            } else {
                saveResult.innerHTML = '<p style="color:#dc3545;font-size:0.8rem;">Kļūda: ' + (result.error || 'Nezināma kļūda') + '</p>';
            }
        } catch (err) {
            saveResult.innerHTML = '<p style="color:#dc3545;font-size:0.8rem;">Kļūda: ' + err.message + '</p>';
        }
    });
});