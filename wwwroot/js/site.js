// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener('DOMContentLoaded', function () {
    // Inputs
    const personasInput = document.querySelectorAll('.input-personas');
    // Contenedores
    const containerPaypal = document.getElementById('paypal-button-container');
    // Botones
    const botonesReservar = document.querySelectorAll('#btn-pq1, #btn-pq2, #btn-pq3, #btn-pq4, #btn-pq5');
    const botonConfirmar = document.getElementById('btnConfirmar');
    const botonCerrarReservar = document.getElementById('btnCerrarReservar');
    const botonCerrarPaypal = document.getElementById('btnCerrarPaypal');
    // Modales
    const modalReservarWin = document.getElementById('modalReservar');
    const modalPaypalWin = document.getElementById('modalPaypal');
    // Campos requeridos
    const camposRequeridos = ['nombre', 'apellidoPaterno', 'correo', 'telefono', 'datePicker', 'timePicker'];
    // Códigos de ejemplo
    const codigosValidos = ['DESC10', 'DESC20', 'DESC30', 'DESC50', 'DESC90'];

    // Calcular costos base en tabla de paquetes
    if (personasInput) {
        personasInput.forEach((input, index) => {
            input.addEventListener('input', function () {
                // Obtener personas adicionales
                const personas = parseInt(input.value) || 0;
                const personasBase = 4;
                const personasAdicionales = Math.max(0, personas - personasBase);
                //Calcular costo adicional
                const costoPersonaAdicional = parseFloat(document.getElementById(`adp-pq${index + 1}`).textContent);
                const costoAdicional = personasAdicionales * costoPersonaAdicional;
                const costoAdicionalCell = document.getElementById(`adc-pq${index + 1}`);
                costoAdicionalCell.textContent = costoAdicional.toFixed(2);
                // Calcular costo total
                const costoBase = parseFloat(document.getElementById(`cst-pq${index + 1}`).textContent);
                const costoTotalCell = document.getElementById(`tot-pq${index + 1}`);
                costoTotalCell.textContent = (costoBase + costoAdicional).toFixed(2);
            });
        });
    }

    // Calcular costo final en modal
    if (botonesReservar) {
        botonesReservar.forEach((button, index) => {
            button.addEventListener('click', function () {
                // Personas adicionales
                const personasInput = document.getElementById(`per-pq${index + 1}`);
                const personas = parseInt(personasInput.value) || 0;
                const personasBase = 4;
                const personasAdicionales = Math.max(0, personas - personasBase);
                // Costos
                const costoBase = parseFloat(document.getElementById(`cst-pq${index + 1}`).textContent);
                const costoPersonaAdicional = parseFloat(document.getElementById(`adp-pq${index + 1}`).textContent);
                const costoAdicional = personasAdicionales * costoPersonaAdicional;
                const costoTotal = costoBase + costoAdicional;
                // Descuento
                const codigoDescInput = document.getElementById('codigoDesc');
                const mensajeDesc = document.getElementById('mensajeDesc');
                // Costos base
                document.getElementById('precioBase').textContent = `$${costoBase.toFixed(2)}`;
                document.getElementById('personaAdicionalTag').textContent = `Persona adicional (${personasAdicionales}): `;
                document.getElementById('personaAdicional').textContent = `$${costoAdicional.toFixed(2)}`;
                document.getElementById('subtotal').textContent = `$${costoTotal.toFixed(2)}`;
                document.getElementById('descuentoAplicable').textContent = `$0.00`;
                document.getElementById('totalDespuesDescuento').textContent = `$${costoTotal.toFixed(2)}`;
                // Calcular descuento
                codigoDescInput.addEventListener('input', function () {
                    const codigoDesc = codigoDescInput.value;
                    let descuento = 0;
                    let esValidoCodigo = codigosValidos.includes(codigoDesc);
                    // Validar código de descuento ingresado
                    if (esValidoCodigo) {
                        if (codigoDesc === 'DESC10') {
                            descuento = 0.10;
                        } else if (codigoDesc === 'DESC20') {
                            descuento = 0.20;
                        } else if (codigoDesc === 'DESC30') {
                            descuento = 0.30;
                        } else if (codigoDesc === 'DESC50') {
                            descuento = 0.50;
                        } else if (codigoDesc === 'DESC90') {
                            descuento = 0.90;
                        }
                        mensajeDesc.textContent = '';
                    } else {
                        mensajeDesc.textContent = 'Código de descuento no válido.';
                    }
                    // Costo final con descuento
                    const montoDescuento = costoTotal * descuento;
                    const totalDespuesDescuento = costoTotal - montoDescuento;
                    document.getElementById('descuentoAplicable').textContent = `$${montoDescuento.toFixed(2)}`;
                    document.getElementById('totalDespuesDescuento').textContent = `$${totalDespuesDescuento.toFixed(2)}`;
                });
                // Mostrar modal Reservar
                const modalReservar = new bootstrap.Modal(document.getElementById('modalReservar'), {
                    backdrop: 'static',
                    keyboard: false
                });
                modalReservar.show();
            });
        });
    }

    // Verificar validez de formulario
    function verificarValidezForm() {
        let esValido = true;
        // Validar que todos los campos requeridos han sido llenados
        camposRequeridos.forEach(idCampo => {
            const campo = document.getElementById(idCampo);
            if (campo) {
                if (!campo.value.trim()) {
                    esValido = false;
                }
            }
        });
        // Cambiar estatus de botón Confirmar
        if (botonConfirmar) {
            botonConfirmar.disabled = !esValido;
        }
    }

    // Escuchadores de eventos
    camposRequeridos.forEach(idCampo => {
        const campo = document.getElementById(idCampo);
        // Agregar escuchador de evento para validar campos de formulario
        if (campo) {
            campo.addEventListener('input', verificarValidezForm);
        }
    })
    if (modalReservarWin) {
        modalReservarWin.addEventListener('shown.bs.modal', function () {
            // Mostrar pickers de fecha y hora
            flatpickr(".datepicker", { dateFormat: "d/m/Y" });
            flatpickr(".timepicker", { enableTime: true, noCalendar: true, dateFormat: "H:i" });
        });
    }
    if (botonCerrarReservar) {
        botonCerrarReservar.addEventListener('click', function () {
            // Limpiar campos del formulario al cerrar modal
            document.getElementById('nombre').value = '';
            document.getElementById('apellidoPaterno').value = '';
            document.getElementById('apellidoMaterno').value = '';
            document.getElementById('correo').value = '';
            document.getElementById('telefono').value = '';
            document.getElementById('datePicker').value = '';
            document.getElementById('timePicker').value = '';
            document.getElementById('codigoDesc').value = '';
            document.getElementById('mensajeDesc').textContent = '';
            document.getElementById('subtotal').textContent = '';
            document.getElementById('descuentoAplicable').textContent = '';
            document.getElementById('totalDespuesDescuento').textContent = '';
            // Ocultar modal
            modalReservarWin.hidden();
        });
    }
    if (botonCerrarPaypal) {
        botonCerrarPaypal.addEventListener('click', function () {
            // Borrar botón PayPal
            containerPaypal.innerHTML = '';
            // Restaurar modal Reservar
            const modalReservar = new bootstrap.Modal(modalReservarWin, {
                backdrop: 'static',
                keyboard: false
            });
            modalReservar.show();
        })
    }

    // Confirmar reservación mostrando botón de pago PayPal
    if (botonConfirmar) {
        // Deshabilitar botón para validar llenado de formulario de reserva
        botonConfirmar.disabled = true;
        // Escuchador de eventos
        botonConfirmar.addEventListener('click', function () {
            // Ocultar modal Reservar
            const modalReservar = bootstrap.Modal.getInstance(modalReservarWin);
            if (modalReservar) {
                modalReservar.hide();
            }
            // Mostrar modal PayPal
            const modalPaypal = new bootstrap.Modal(modalPaypalWin, {
                backdrop: 'static',
                keyboard: false
            });
            modalPaypal.show();
            //Mostrar botón PayPal en modal
            paypal.Buttons({
                createOrder: function (data, actions) {
                    return actions.order.create({
                        purchase_units: [{
                            amount: {
                                value: document.getElementById('totalDespuesDescuento').textContent.replace('$', '')
                            }
                        }]
                    });
                },
                onApprove: function (data, actions) {
                    return actions.order.capture().then(function (details) {
                        alert("Transacción completada por " + details.payer.name.given_name);
                    });
                }
            }).render('#paypal-button-container');
        });
    }
});
