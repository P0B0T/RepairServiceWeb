function removeExtraSpaces(element) {
    var inputValue = element.value;
    inputValue = inputValue.replace(/\s{2,}/g, ' ').trim();
    element.value = inputValue;
}