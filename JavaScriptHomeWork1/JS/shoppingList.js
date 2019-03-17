'use strict'

var shoppingList = [
    { name: "Молоко", quantity: 3, paid: true },
    { name: "Чай", quantity: 1, paid: false },
    { name: "Сахар", quantity: 2, paid: false },
    { name: "Хлеб", quantity: 2, paid: false },
    { name: "Печенье", quantity: 8, paid: false },
    { name: "Сок(пак)", quantity: 2, paid: false },   
    { name: "Пиво", quantity: 3, paid: false },   
    { name: "Йогурт", quantity: 2, paid: true },   
    { name: "Овсянка", quantity: 5, paid: true },   
    { name: "Мыло", quantity: 1, paid: false },   
    { name: "Пакет", quantity: 1, paid: true }   
]

shoppingList.sortedPrint = function() {
    document.write("Купленные товары: <br/><br/>");
    
    this.forEach(function(item){
        if(item.paid)
        document.write(item.name + " : "+
                    item.quantity +"шт.<br/>");    
    });
                 
     document.write("<br/> Не купленные товары: <br/><br/>"); 
    
     this.forEach(function(item){
        if(!item.paid)
        document.write(item.name + " : " +
                    item.quantity +"шт.<br/>");    
    });
}

shoppingList.AddProduct = function(name) {
    
  let flag = false;
    
  this.forEach(function(item){
     if(item.name == name){
         item.quantity++;
         flag = true;    
         return;
     } 
  });
    
    if(flag) return;
    
    var obj = { name: name, quantity: 1, paid: false };  
   this.push(obj);     
}

shoppingList.BuyProduct = function(name){
    this.forEach(function(item){
      if(item.name == name)
     item.paid = true;              
 });
}

shoppingList.AddProduct("Консерва");
shoppingList.AddProduct("Консерва");
shoppingList.AddProduct("Консерва");
shoppingList.BuyProduct("Консерва");
shoppingList.sortedPrint();


