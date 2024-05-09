// Функция для удаления лишних пробелов
function removeExtraSpaces(element) {

    var inputValue = element.value; // Получение значения элемента

    inputValue = inputValue.replace(/\s{2,}/g, ' ').trim(); // Замена двух или более пробелов на один пробел и удаление пробелов в начале и конце строки

    element.value = inputValue; // Присвоение нового значения элементу
}