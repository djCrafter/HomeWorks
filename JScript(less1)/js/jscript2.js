function Vmonth(obj){
  var a=obj.zn.value;
  var s;
  switch (parseInt(a))
  {
    case 1: s="Январь"; break;
    case 2: s="Февраль"; break;
    case 3: s="Март"; break;
    case 4: s="Апрель"; break;
    case 5: s="Май"; break;
    case 6: s="Июнь"; break;
    case 7: s="Июль"; break;
    case 8: s="Август"; break;
    case 9: s="Сентябрь"; break;
    case 10: s="Октябрь"; break;
    case 11: s="Ноябрь"; break;      
    case 12: s="Декабрь"; break;      
  
   default: s='Неверно указан месяц'
  }
  obj.res.value=s;
}
