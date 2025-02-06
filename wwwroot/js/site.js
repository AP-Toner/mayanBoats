// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener('DOMContentLoaded', () => {
    // Constantes
    const personasBase = 4;
    const camposRequeridos = ['nombre', 'apellidoPaterno', 'correo', 'telefono', 'datePicker', 'timePicker'];
    const codigosValidos = ['DESC10', 'DESC20', 'DESC30', 'DESC50', 'DESC90', 'FREE'];

    // Elementos
    const personasInput = document.querySelectorAll('.input-personas');
    const containerPaypal = document.getElementById('paypal-button-container');
    const botonesReservar = document.querySelectorAll('.btnReservar');
    const botonConfirmar = document.getElementById('btnConfirmar');
    const botonCerrarReservar = document.getElementById('btnCerrarReservar');
    const botonCerrarPaypal = document.getElementById('btnCerrarPaypal');
    const modalReservarWin = document.getElementById('modalReservar');
    const modalPaypalWin = document.getElementById('modalPaypal');

    // Funciones
    const calcularCostos = (index, personas) => {
        const personasAdicionales = Math.max(0, personas - personasBase);
        const costoPersonaAdicional = parseFloat(document.getElementById(`adp-pq-${index}`).textContent);
        const costoAdicional = personasAdicionales * costoPersonaAdicional;
        document.getElementById(`adc-pq-${index}`).textContent = costoAdicional.toFixed(2);

        const costoBase = parseFloat(document.getElementById(`cst-pq-${index}`).textContent);
        document.getElementById(`tot-pq-${index}`).textContent = (costoBase + costoAdicional).toFixed(2);
    };

    const verificarValidezForm = () => {
        let esValido = camposRequeridos.every(idCampo => {
            const campo = document.getElementById(idCampo);
            return campo && campo.value.trim();
        });
        if (botonConfirmar) {
            botonConfirmar.disabled = !esValido;
        }
    };

    const aplicarDescuento = (costoTotal, codigoDesc) => {
        let descuento = 0;
        if (codigosValidos.includes(codigoDesc)) {
            switch (codigoDesc) {
                case 'DESC10': descuento = 0.10; break;
                case 'DESC20': descuento = 0.20; break;
                case 'DESC30': descuento = 0.30; break;
                case 'DESC50': descuento = 0.50; break;
                case 'DESC90': descuento = 0.90; break;
                case 'FREE': descuento = 0.99; break;
            }
            document.getElementById('mensajeDesc').textContent = '';
        } else {
            document.getElementById('mensajeDesc').textContent = 'Código de descuento no válido.';
            return;
        }
        const montoDescuento = costoTotal * descuento;
        document.getElementById('descuentoAplicable').textContent = `$${montoDescuento.toFixed(2)}`;
        document.getElementById('totalDespuesDescuento').textContent = `$${(costoTotal - montoDescuento).toFixed(2)}`;
    };

    // Escuchadores de Eventos
    personasInput.forEach((input, index) => {
        input.addEventListener('input', () => {
            const personas = parseInt(input.value) || 0;
            calcularCostos(index, personas);
        });
    });

    botonesReservar.forEach((button, index) => {
        button.addEventListener('click', () => {
            const personas = parseInt(document.getElementById(`per-pq-${index}`).value) || 0;
            const costoBase = parseFloat(document.getElementById(`cst-pq-${index}`).textContent);
            const costoPersonaAdicional = parseFloat(document.getElementById(`adp-pq-${index}`).textContent);
            const costoAdicional = Math.max(0, personas - personasBase) * costoPersonaAdicional;
            const costoTotal = costoBase + costoAdicional;
            const nombrePaquete = document.getElementById(`nmb-pq-${index}`).textContent.trim();

            document.getElementById('precioBase').textContent = `$${costoBase.toFixed(2)}`;
            document.getElementById('personaAdicionalTag').textContent = `Persona adicional (${Math.max(0, personas - personasBase)}): `;
            document.getElementById('personaAdicional').textContent = `$${costoAdicional.toFixed(2)}`;
            document.getElementById('subtotal').textContent = `$${costoTotal.toFixed(2)}`;
            document.getElementById('descuentoAplicable').textContent = '$0.00';
            document.getElementById('totalDespuesDescuento').textContent = `$${costoTotal.toFixed(2)}`;

            document.getElementById('codigoDesc').addEventListener('input', (e) => {
                aplicarDescuento(costoTotal, e.target.value);
            });

            document.getElementById('paypalNombrePaquete').textContent = `${nombrePaquete} horas`;

            const modalReservar = new bootstrap.Modal(modalReservarWin, { backdrop: 'static', keyboard: false });
            modalReservar.show();
        });
    });

    camposRequeridos.forEach(idCampo => {
        const campo = document.getElementById(idCampo);
        if (campo) {
            campo.addEventListener('input', verificarValidezForm);
        }
    });

    if (modalReservarWin) {
        modalReservarWin.addEventListener('shown.bs.modal', () => {
            flatpickr(".datepicker", { dateFormat: "d/m/Y" });
            flatpickr(".timepicker", {
                enableTime: true,
                noCalendar: true,
                dateFormat: "H:i",
                time_24hr: true
            });
        });
    }

    if (botonCerrarReservar) {
        botonCerrarReservar.addEventListener('click', () => {
            camposRequeridos.forEach(idCampo => {
                const campo = document.getElementById(idCampo);
                if (campo) campo.value = '';
            });
            document.getElementById('codigoDesc').value = '';
            document.getElementById('mensajeDesc').textContent = '';
            document.getElementById('subtotal').textContent = '';
            document.getElementById('descuentoAplicable').textContent = '';
            document.getElementById('totalDespuesDescuento').textContent = '';
            botonConfirmar.disabled = true;
            modalReservarWin.hidden = true;
        });
    }

    if (botonCerrarPaypal) {
        botonCerrarPaypal.addEventListener('click', () => {
            containerPaypal.innerHTML = '';
            const modalReservar = new bootstrap.Modal(modalReservarWin, { backdrop: 'static', keyboard: false });
            modalReservar.show();
        });
    }

    if (botonConfirmar) {
        botonConfirmar.disabled = true;
        botonConfirmar.addEventListener('click', () => {
            const modalReservar = bootstrap.Modal.getInstance(modalReservarWin);
            if (modalReservar) modalReservar.hide();

            // Información del cliente
            const nombreCliente = document.getElementById('nombre').value + ' ' + document.getElementById('apellidoPaterno').value + ' ' + document.getElementById('apellidoMaterno').value;
            const correo = document.getElementById('correo').value;
            const telefono = document.getElementById('telefono').value;

            // Detalles paquete
            const fecha = document.getElementById('datePicker').value;
            const hora = document.getElementById('timePicker').value;

            // Resumen de venta
            const precioBase = document.getElementById('precioBase').textContent;
            const personaAdicionalTag = document.getElementById('personaAdicionalTag').textContent;
            const personaAdicional = document.getElementById('personaAdicional').textContent;
            const subtotal = document.getElementById('subtotal').textContent;
            const descuentoAplicable = document.getElementById('descuentoAplicable').textContent;
            const totalDespuesDescuento = document.getElementById('totalDespuesDescuento').textContent;

            // Llenar campos del modal
            document.getElementById('paypalNombreCliente').textContent = nombreCliente;
            document.getElementById('paypalCorreo').textContent = correo;
            document.getElementById('paypalTelefono').textContent = telefono;
            document.getElementById('paypalFecha').textContent = fecha;
            document.getElementById('paypalHora').textContent = hora;
            document.getElementById('paypalPrecioBase').textContent = precioBase;
            document.getElementById('paypalPersonaAdicionalTag').textContent = personaAdicionalTag;
            document.getElementById('paypalPersonaAdicional').textContent = personaAdicional;
            document.getElementById('paypalSubtotal').textContent = subtotal;
            document.getElementById('paypalDescuentoAplicable').textContent = descuentoAplicable;
            document.getElementById('paypalTotalDespuesDescuento').textContent = totalDespuesDescuento;

            const modalPaypal = new bootstrap.Modal(modalPaypalWin, { backdrop: 'static', keyboard: false });
            modalPaypal.show();

            paypal.Buttons({
                createOrder: (data, actions) => {
                    return actions.order.create({
                        purchase_units: [{
                            amount: {
                                currency_code: 'USD',
                                value: parseFloat(document.getElementById('totalDespuesDescuento').textContent.replace('$', ''))
                            }
                        }]
                    });
                },
                onApprove: (data, actions) => {
                    return actions.order.capture().then(details => {
                        alert(`Transacción completada por ${details.payer.name.given_name}`);

                        // Enviar detalles al backend
                        fetch('/api/pago/capture', {
                            method: 'POST',
                            headers: {
                                'Content-Type': 'application/json'
                            },
                            body: JSON.stringify(details)
                        })
                            .then(response => response.json())
                            .then(serverResponse => {
                                console.log('Transacción almacenada: ', serverResponse);
                            })
                            .catch(error => {
                                console.error('Error almacenando los detalles de la transacción: ', error);
                            });
                    });
                }
            }).render('#paypal-button-container');
        });
    }
});
