function changeText(buttonNumber) {
    var newCity, newCountry, newTemp, newCan, lol;
    var newCity_2, newCountry_2, newTemp_2, newCan_2;

    if (buttonNumber === 1) {
        newCity = 'Санкт-Петербург';
        newCountry = '<img src="{% static \'main/icon/russia.webp\' %}" alt="Логотип">\n';
        newTemp = ' А';
        newCan = 'Посадка окончена';

        newCity_2 = 'Дубай';
        newCountry_2 = ` <img src="{% static 'main/js/tablo.js' %}" alt="Логотип"> Turkish Airlance`;
        newTemp_2 = ' А';
        newCan_2 = 'Ожидание посадки';
        lol = 'вылета';

    } else if (buttonNumber === 2) {
        newCity = 'Москва';
        newCountry = ' <img src="{% static \'main/icon/russia.webp\' %}" alt="Логотип"> Россия';
        newTemp = ' А';
        newCan = 'Ожидание прилета';

        newCity_2 = 'Анталья';
        newCountry_2 = ' <img src="{% static \'main/js/tablo.js\' %}" alt="Логотип"> Pegasus';
        newTemp_2 = 'А';
        newCan_2 = 'Ожидание прилета';
        lol = 'прилета';

    }

    setElementContent('element2', newCity, 'element3', newCountry, 'element4', newTemp, 'element5', newCan, 'element12', lol);
    setElementContent('element7', newCity_2, 'element8', newCountry_2, 'element9', newTemp_2, 'element10', newCan_2, 'element12', lol);
}

function setElementContent(id1, content1, id2, content2, id3, content3, id4, content4, id5, content5) {
    document.getElementById(id1).innerHTML = content1;
    document.getElementById(id2).innerHTML = content2;
    document.getElementById(id3).innerHTML = content3;
    document.getElementById(id4).innerHTML = content4;
    document.getElementById(id5).innerHTML = content5;

}


function padZero(number) {
    return (number < 10 ? '0' : '') + number;
}

// Вызываем функцию updateDateTime каждую секунду (1000 миллисекунд)
setInterval(updateDateTime, 1000);

// Вызываем функцию сразу, чтобы отобразить текущее время
updateDateTime();


// Переключение кнопок
function toggleButton(buttonNumber) {
    var button1 = document.getElementById('button1');
    var button2 = document.getElementById('button2');

    if (buttonNumber === 1) {
        button1.classList.add('button-disabled');
        button1.disabled = true;

        button2.classList.remove('button-disabled');
        button2.disabled = false;
    } else if (buttonNumber === 2) {
        button2.classList.add('button-disabled');
        button2.disabled = true;

        button1.classList.remove('button-disabled');
        button1.disabled = false;
    }
}

// Вызываем функцию toggleButton при загрузке страницы для установки начального состояния
document.addEventListener('DOMContentLoaded', function () {
    toggleButton(1); // Нажимаем первую кнопку при загрузке
});
