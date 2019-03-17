'use strict'

function stringStat(str){
  let numbers = 0;
  let latters = 0;
  let others = 0;
    
  for(let i = 0; i < str.length; ++i) {
      if(/[0-9]/.test(str[i])){
        numbers++;  
      }
      else if(/[a-zA-Zа-яА-Я]/.test(str[i])){
          latters++;
      }
      else{
          others++;
      }
  }
    
    document.write("Numbers: " + numbers + "<br/>");
    document.write("Latters: " + latters + "<br/>");
    document.write("Others: " + others + "<br/>");
}
    
    var str = "Hello World 1234 привет привет.";

stringStat(str);