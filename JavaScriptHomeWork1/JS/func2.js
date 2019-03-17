'use strict'

function digitToString(digit){
    var units = [ "Ноль", "Один", "Два", "Три", "Четыре", "Пять",
                "Шесть", "Семь", "Восемь", "Девять"];
    
    var fromTenToTwenty = ["Десять", "Одинадцать", "Двенадцать", "Тринадцать", "Четырнадцать",
    "Пятнадцать", "Шестнадцать", "Семнадцать", "Восемнадцать", 
                           "Девятнадцать"];
    
    var tens = ["", "", "Двадцать", "Тридцать", "Сорок", "Пятдесят",
               "Шестьдесят", "Семдесят", "Восемдесят", "Девяносто"];           
    
    if(digit < 10){
        return units[digit];
    }
    else if(digit >= 10 && digit < 20){
        return fromTenToTwenty[digit - 10];
    }
    else{          
    var sNumber = digit.toString();
    
     return  tens[parseInt(sNumber[0])] + " " 
                   + units[parseInt(sNumber[1])];        
  }
}


document.write(digitToString(51));

