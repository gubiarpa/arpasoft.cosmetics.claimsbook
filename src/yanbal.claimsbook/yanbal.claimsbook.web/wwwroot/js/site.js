/**
 * Construye una etiqueta <option> en texto
 * dataValue : Valor para el atributo value
 * displayValue : Valor visualizado por el usuario
 */
function printOptionInSelect(dataValue, displayValue) {
    return `<option ${ dataValue == null ? 'disabled selected' : ''} value='${dataValue}'>${displayValue}</option>`;
}