/**
 * Construye una etiqueta <option> en texto
 * dataValue : Valor para el atributo value
 * displayValue : Valor visualizado por el usuario
 */
function printOptionInSelect(dataValue, displayValue) {
    return `<option ${ dataValue == null ? 'disabled selected' : ''} value=${quoteNull(dataValue)}>${displayValue}</option>`;
}

function quoteNull(value) {
    return value == null ? null : `'${value}'`;
}

function buildEndpoint(methodname) {
    return `${window.location.href}${window.location.href.substr(window.location.href.length - 1, 1) == '/' ? '' : '/'}${methodname}`;
}