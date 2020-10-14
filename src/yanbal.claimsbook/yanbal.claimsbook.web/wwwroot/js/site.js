/*
 * HELPERS
 */
/**
 * Construye una etiqueta <option> en texto
 * dataValue : Valor para el atributo value
 * displayValue : Valor visualizado por el usuario
 */
function printOptionInSelect(dataValue, displayValue) {
    return `<option ${ dataValue == null ? 'disabled selected' : ''} value=${quoteNull(dataValue)}>${displayValue}</option>`;
}

function enableControls(triggerControl, affectedControls) {
    let _triggerControl = $(`#${triggerControl}`);
    if (_triggerControl != null) {
        for (var affectedControl of affectedControls) {
            $(`#${affectedControl}`).removeAttr('disabled');
        }
    }
}

function changeInputByDocumentType(triggerControl, affectedControls) {
    let _triggerControl = $(`#${triggerControl}`);
    let _triggerText = _triggerControl[0][_triggerControl[0].selectedIndex].innerHTML;
    if (_triggerControl != null) {
        for (var affectedControl of affectedControls) {
            let objAffectedControl = $(`#${affectedControl}`);
            if (_triggerText == 'DNI') {
                objAffectedControl.attr('maxlength', '8');
            }
            else {
                objAffectedControl.removeAttr('maxlength');
            }
        }
    }
}

/*
 * AUXILIAR
 */

function enablePersonalInfoForm() {
    let documentTypeValue = $('#selectDocumentType').val();
    let answerTypeValue = $('#selectAnswerType').val();
    let isAdult = $('#checkIsYounger')[0].checked;
    let guardDocumentTypeValue = $('#selectGuardDocumentType').val();

    let eval = (
        (documentTypeValue != null && answerTypeValue != null) &&
        (!isAdult || guardDocumentTypeValue != null)
    );
    if (eval) {
        $('[data-parentForm="personalInfoForm"]').removeAttr('disabled');
    }
}

function enableContractedGoodForm() {
    let contractedGoodForm = getDataForm('contractedGoodForm');
    let eval = ( // true: si el formulario está correctamente completado
        (contractedGoodForm.type.isService || contractedGoodForm.type.isProduct) &&
        (contractedGoodForm.claimedAmount.val().length > 0) &&
        (contractedGoodForm.description.val().length > 0)
    );
    let button = $('[data-parentForm="contractedGoodForm"]')
    if (eval) {
        button.removeAttr('disabled');
    } else {
        button.attr('disabled', 'disabled');
    }
}

function quoteNull(value) {
    return value == null ? null : `'${value}'`;
}

function buildEndpoint(methodname) {
    return `${window.location.href}${window.location.href.substr(window.location.href.length - 1, 1) == '/' ? '' : '/'}${methodname}`;
}

/* Validate Mail */
function validateEmail(mail) {
    return (/^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/).test(mail);
}

function zeroPad(num, places) {
    return String(num).padStart(places, '0');
}

/* Validate Decimal */
function validateDecimal(number) {
    return (/^\d+(\.\d{1,2})?$/).test(number);
}