'use strict'

function changeRegister(str) {
    
    var result = "";
      
   for(let i = 0; i < str.length; ++i){
       if(/[a-zа-я]/.test(str[i])){       
          result += str[i].toUpperCase();          
       }
       else if(/[A-ZА-Я]/.test(str[i])){      
          result += str[i].toLowerCase();          
       }
       else if(/[0-9]/.test(str[i])){
           result += "_";
       }
       else{
           result += str[i];
      }
   } 
    return result;
}

var str = "Hello World 123";

changeRegister(str);

document.write(changeRegister(str));
