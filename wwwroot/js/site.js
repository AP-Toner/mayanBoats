// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener('DOMContentLoaded', function () {
    const inputPersonas = document.querySelectorAll('.input-personas');
    const botonesReservar = document.querySelectorAll('#btn-pq1, #btn-pq2, #btn-pq3, #btn-pq4, #btn-pq5');

    if (inputPersonas) {
        inputPersonas.forEach((input, index) => {
            input.addEventListener('input', function () {
                const personas = parseInt(input.value) || 0;
                const personasBase = 4;

                const personasAdicionales = Math.max(0, personas - personasBase);

                const costoPersonaAdicional = parseFloat(document.getElementById(`adp-pq${index + 1}`).textContent);

                const costoAdicional = personasAdicionales * costoPersonaAdicional;

                const costoAdicionalCell = document.getElementById(`adc-pq${index + 1}`);
                costoAdicionalCell.textContent = costoAdicional.toFixed(2);

                const costoBase = parseFloat(document.getElementById(`cst-pq${index + 1}`).textContent);

                const costoTotalCell = document.getElementById(`tot-pq${index + 1}`);
                costoTotalCell.textContent = (costoBase + costoAdicional).toFixed(2);
            });
        });
    }

    if (botonesReservar) {
        botonesReservar.forEach((button, index) => {
            button.addEventListener('click', function () {
                const costoTotal = document.getElementById(`tot-pq${index + 1}`).textContent;
                document.getElementById('totalResumen').textContent = `$${costoTotal}`;
                const modalReservar = new bootstrap.Modal(document.getElementById('modalReservar'));
                modalReservar.show();
            });
        });
    }
});
