function summa(obj) {
     var summa=0;
     var a1=1*obj.a1.value;
     var a2=1*obj.a2.value;
     if (a2 > a1){
      for (var i = a1; i <= a2; i++) {
        summa+=i;
      }
      obj.result.value = summa;
     }
     else
      alert("Значение ОТ должно быть меньше значения ДО")
    }
 
