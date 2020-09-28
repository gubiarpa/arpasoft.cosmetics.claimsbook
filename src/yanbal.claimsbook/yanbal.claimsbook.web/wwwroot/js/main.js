
/* Document Loaded */
$(document).ready(() => {
    
    /* Document Types */
    $.ajax({
        type: 'GET',
        url: buildEndpoint('Claims/GetDocumentTypes'),
        success(array) {
            let options = printOptionInSelect(null, 'Tipo de Documento');
            for (let elem of array) {
                options += printOptionInSelect(elem.id, elem.description);
            }
            $('[data-id="documentType"]').html(options).removeAttr('disabled');
        }
    });

    $('[data-id="documentType"]').change(function () {
        let triggerControl = $(this).attr('id');
        let affectedControls = $(this).attr('data-for').split(',');
        let affectedLengthControls = $(this).attr('data-length').split(',');
        enableControls(triggerControl, affectedControls);
        changeInputByDocumentType(triggerControl, affectedLengthControls);
        enablePersonalInfoForm();
    });

    /* Answer Types */
    $.ajax({
        type: 'GET',
        url: buildEndpoint('Claims/GetAnswerTypes'),
        success(array) {
            let options = printOptionInSelect(null, 'Tipo de Respuesta');
            for (let elem of array) {
                options += printOptionInSelect(elem.id, elem.description);
            }
            $('[data-id="answerType"]').html(options);
            $('#selectAnswerType').removeAttr('disabled');
        }
    });

    $('[data-id="answerType"]').change(function () {
        $('#selectGuardAnswerType').val($(this).val());
        let triggerControl = $(this).attr('id');
        let affectedControls = $(this).attr('data-for').split(',');
        enableControls(triggerControl, affectedControls);
        enablePersonalInfoForm();
    });

    /* Departments */
    $.ajax({
        type: 'GET',
        url: buildEndpoint('Claims/GetDepartments'),
        success(array) {
            let options = printOptionInSelect(null, 'Departamento');
            for (let elem of array) {
                options += printOptionInSelect(elem.code, elem.department);
            }
            $('#selectDepartment').html(options);
            $('#selectGuardDepartment').html(options);
        }
    });

    $.ajax({
        type: 'GET',
        url: buildEndpoint('Claims/GetCompanyInfo'),
        success(result) {
            $('#companyName').html(result.name);
            $('#companyDocumentNumber').html(result.documentNumber);
            $('#companyAddress').html(result.address);
        }
    });

    $('#selectDepartment').change(function () {
        changeGeoZone({
            selectSubzone: 'selectDistrict',
            subZoneDefault: 'Distrito',
            ajaxMethod: 'GET',
            url: buildEndpoint('Claims/GetProvinces'),
            body: $(this).val(),
            zoneDefault: 'Provincia',
            selectZone: 'selectProvince'
        }, function (elem) {
            return printOptionInSelect(elem.code, elem.province);
        }, { code: null, province: 'Provincia' }, { code: null, province: 'Distrito' })
    });

    $('#selectGuardDepartment').change(function () {
        changeGeoZone({
            selectSubzone: 'selectGuardDistrict',
            subZoneDefault: 'Distrito',
            ajaxMethod: 'GET',
            url: buildEndpoint('Claims/GetProvinces'),
            body: $(this).val(),
            zoneDefault: 'Provincia',
            selectZone: 'selectGuardProvince'
        }, function (elem) {
            return printOptionInSelect(elem.code, elem.province);
        }, { code: null, province: 'Provincia' }, { code: null, province: 'Distrito' }
        )
    });

    /* Provinces */
    $('#selectProvince').change(function () {
        changeGeoZone({
            selectSubzone: 'selectDistrict',
            subZoneDefault: null,
            ajaxMethod: 'GET',
            url: buildEndpoint('Claims/GetDistricts'),
            body: $(this).val(),
            zoneDefault: 'Distrito',
            selectZone: 'selectDistrict'
        }, function (elem) {
            return printOptionInSelect(elem.code, elem.district);
        }, { code: null, district: 'Distrito' }, { code: null, district: null })
    });

    $('#selectGuardProvince').change(function () {
        changeGeoZone({
            selectSubzone: 'selectGuardDistrict',
            subZoneDefault: null,
            ajaxMethod: 'GET',
            url: buildEndpoint('Claims/GetDistricts'),
            body: $(this).val(),
            zoneDefault: 'Distrito',
            selectZone: 'selectGuardDistrict'
        }, function (elem) {
            return printOptionInSelect(elem.code, elem.district);
        }, { code: null, district: 'Distrito' }, { code: null, district: null })
    });

    /* Districts */
    $('#selectDistrict').change(function () {

    });

    /* Younger */
    $('#checkIsYounger').change(function () {
        let guardForm = $('#guardForm');
        setTimeout(() => {
            guardForm.toggleClass('not-display');
            guardForm.toggleClass('invisible');
            guardForm.addClass('animate__fadeIn');
        }, 200);
        $("html, body").animate({ scrollTop: $(document).height() }, 700);

    });

    /* Continue */
    $('[name="btnContinue"]').click(function () {

        /// (i) Cambio de formulario
        let parentFormName = $(this).attr('data-parentForm');
        let nextFormName = $(this).attr('data-nextForm');

        let validationResult = validateForm(parentFormName);

        if (validationResult.length == 0) {
            $('#' + parentFormName).toggleClass('not-display');
            $('#' + nextFormName).toggleClass('not-display animate__animated animate__fadeIn');
        }

        /// (ii) Línea de progreso
        let selectorStr = `[data-type="lineProgress"][data-form="${nextFormName}"]`;
        $(selectorStr).attr('checked', 'checked');


        /// (iii) Actualización de Resumen Final
        updateSummary();
    });

/* Check Agree */
    $('[name="checkAgree"]').change(function () {
        let agreeList = $('[name="checkAgree"]');
        let isEnabled = agreeList[0].checked && agreeList[1].checked;
        let btnSend = $('#btnSend');
        if (isEnabled) {
            btnSend.removeAttr('disabled');
        }
        else {
            btnSend.attr('disabled', 'disabled');
        }
    });

    /* Send */
    $('#btnSend').click(sendForm);

    $('#btnPdfGenerator').click(openPdf);
});

/* Change Department */
const changeGeoZone = (obj, printerMethod, defaultElem, defaultSubElem) => {

    $(`#${obj.selectSubzone} option`).remove();
    $(`#${obj.selectSubzone}`).attr('disabled', 'disabled');
    $(`#${obj.selectSubzone}`).html(printerMethod(defaultSubElem));
    $.ajax({
        type: obj.ajaxMethod,
        url: obj.url,
        data: { geoCode: obj.body },
        success(array) {
            let options = printerMethod(defaultElem);
            for (let elem of array) {
                options += printerMethod(elem)
            }
            $(`#${obj.selectZone}`).html(options).removeAttr('disabled');
        }
    });
}

/* Validation */
const validateForm = (nameForm) => {

    let errors = [];

    switch (nameForm) {
        case 'personalInfoForm':
            // Document Type
            let documentTypeID = $('#selectDocumentType option:selected').val();
            let documentTypeName = $('#selectDocumentType option:selected').text();
            let textDocumentNumber = $('#textDocumentNumber').val();

            if (documentTypeID == null || documentTypeID == 'null') {
                $('#selectDocumentType').addClass('border-danger');
                //errors.push({ title: 'DNI' });
            }
            else {
                $('#selectDocumentType').removeClass('border-danger');
            }

            if (documentTypeName == 'DNI') {
                if (textDocumentNumber == null || textDocumentNumber.length != 8) {
                    $('#textDocumentNumber').removeClass('border-danger');
                }
                // errors.push({ title: 'DNI', message: 'El DNI debe ser numérico y de 8 dígitos' })
            }
            break;
        case 'contractedGoodForm':
            break;
        case 'claimDetailForm':
            break;
        case 'finalSummaryForm':
            break;
        default:
            break;
    }

    return errors;
}

/* Summary Update */
const updateSummary = () => {

    /// (i) Información del Consumidor Reclamante
    let sumDocument = `${$('#selectDocumentType option:selected').text()} ${$('#textDocumentNumber').val()}`;
    let sumFullName = `${$('#textClaimerName').val()} ${$('#textSurnameFather').val()} ${$('#textSurnameMother').val()}`;
    let sumPhoneNumber = `${$('#textTelephone').val()}`;
    let sumAnswerType = `${$('#selectAnswerType option:selected').text()}`;
    let sumEMail = `${$('#textMail').val()}`;
    let sumFullAddress = `${$('#textAddress').val()} ${$('#selectDistrict option:selected').text()}, ${$('#selectProvince option:selected').text()}, ${$('#selectDepartment option:selected').text()}`;
    let sumIsAdult = `${$('#checkIsYounger')[0].checked ? 'No' : 'Sí'}`;

    /// (ii) Información del Apoderado
    let sumGuardDocument = `${$('#selectGuardDocumentType option:selected').text()} ${$('#textGuardDocumentNumber').val()}`;
    let sumGuardFullName = `${$('#textGuardClaimerName').val()} ${$('#textGuardSurnameFather').val()} ${$('#textGuardSurnameMother').val()}`;
    let sumGuardPhoneNumber = `${$('#textGuardTelephone').val()}`;
    let sumGuardEmail = `${$('#textGuardMail').val()}`;
    let sumGuardFullAddress = `${$('#textGuardAddress').val()} ${$('#selectGuardDistrict option:selected').text()}, ${$('#selectGuardProvince option:selected').text()}, ${$('#selectGuardDepartment option:selected').text()}`;

    /// (iii) Información del Bien Contratado
    let sumGoodType = `${$('#checkIsProduct')[0].checked ? 'Producto' : 'Servicio'}`;
    let sumClaimedAmount = `S/ ${$('#textClaimedAmount').val()}`;
    let sumDescription = `${$('#textDescription').val()}`;

    /// (iv) Detalle del Reclamo
    let sumClaimType = $('#checkIsClaim')[0].checked ? 'Reclamo' : 'Queja';
    let sumClaimDetail = $('#textClaimDetail').val();
    let sumOrderDetail = $('#textOrderDetail').val();

    /// [Impresión de valores]
    $('#sumDocument').html(sumDocument);
    $('[name="sumFullName"]').html(sumFullName); // modal
    $('[name="ClaimOrComplaint"]').html(sumClaimType); // modal
    $('#sumPhoneNumber').html(sumPhoneNumber);
    $('[name="sumAnswerType"]').html(sumAnswerType); // same value for main and guard
    $('#sumEMail').html(sumEMail);
    $('#sumFullAddress').html(sumFullAddress);
    $('#sumIsAdult').html(sumIsAdult);

    if (sumIsAdult == 'Sí') { // Hide/Show Section
        $('#block-isNotAdult').addClass('not-display');
    }

    $('#sumGoodType').html(sumGoodType);
    $('#sumClaimedAmount').html(sumClaimedAmount);
    $('#sumDescription').html(sumDescription);

    $('#sumClaimType').html(sumClaimType);
    $('#sumClaimDetail').html(sumClaimDetail);
    $('#sumOrderDetail').html(sumOrderDetail);

    $('#sumGuardDocument').html(sumGuardDocument);
    $('#sumGuardFullName').html(sumGuardFullName);
    $('#sumGuardPhoneNumber').html(sumGuardPhoneNumber);
    $('#sumGuardEmail').html(sumGuardEmail);
    $('#sumGuardFullAddress').html(sumGuardFullAddress);
};

/* Sending */
const sendForm = () => {

    /// (i) Personal Info
    let mainClaimer = {
        documentType: $('#selectDocumentType').val(),
        documentNumber: $('#textDocumentNumber').val(),
        names: $('#textClaimerName').val(),
        paternalSurname: $('#textSurnameFather').val(),
        maternalSurname: $('#textSurnameMother').val(),
        phoneNumber: $('#textTelephone').val(),
        answerType: $('#selectAnswerType option:selected').val(),
        eMail: $('#textMail').val(),
        address: $('#textAddress').val(),
        geoZone: $('#selectDistrict').val()
    };
    let isAdult = !$('#checkIsYounger')[0].checked;
    let guardClaimer = {
        documentType: $('#selectGuardDocumentType').val(),
        documentNumber: $('#textGuardDocumentNumber').val(),
        names: $('#textGuardClaimerName').val(),
        paternalSurname: $('#textGuardSurnameFather').val(),
        maternalSurname: $('#textGuardSurnameMother').val(),
        phoneNumber: $('#textGuardTelephone').val(),
        answerType: $('#selectAnswerType option:selected').val(),
        eMail: $('#textGuardMail').val(),
        address: $('#textGuardAddress').val(),
        geoZone: $('#selectGuardDistrict').val()
    };

    /// (ii) Contracted Good
    let contractedGood = {
        isAProduct: $('#checkIsProduct')[0].checked, // or a service
        claimedAmount: parseFloat($('#textClaimedAmount').val()).toFixed(2),
        goodDescription: $('#textDescription').val()
    };

    /// (iii) Claim Detail
    let claimDetail = {
        isAClaim: $('#checkIsClaim')[0].checked, // or a complaint (queja)
        claimDetail: $('#textClaimDetail').val(),
        orderDetail: $('#textOrderDetail').val()
    };

    /// (iv) Group Information
    let claim = {
        isAdult,
        mainClaimer,
        guardClaimer,
        contractedGood,
        claimDetail
    };

    /// (v) Consuming API
    $.ajax({
        type: 'POST',
        url: buildEndpoint('Claims/SaveClaim'),
        data: claim,
        success(result) {
            $('[name="checkAgree"]').attr('disabled', 'disabled');
            $('#btnSend').attr('disabled', 'disabled');
            $('[name="claimSheetNumber"]').html(`${result.yearNumber}-${zeroPad(result.serialNumber, 4)}`);
            $('#btnPdfGenerator').attr('data-value', result.id).html('Imprimir').removeAttr('disabled');
        },
        error(xhr, ajaxOptions, thrownError) {
            console.log(xhr.status);
            console.log(thrownError);
        }
    });

};

const openPdf = () => {
    let id = $('#btnPdfGenerator').attr('data-value');
    window.open(buildEndpoint('Claims/GenerateClaimPdf/') + id, '_blank');
}
