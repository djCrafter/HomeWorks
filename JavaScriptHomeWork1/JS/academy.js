var academy = [
    { name: "Синий зал", places: 20, faculty: "Металлургический" },
    { name: "Старая аудиторяи", places: 15, faculty: "Металлургический" },
    { name: "Печная комната", places: 10, faculty: "Металлургический" },
    { name: "МатКаб", places: 12, faculty: "Математический" },
    { name: "МатКаб 2", places: 10, faculty: "Математический" },
    { name: "Аудитория 12", places: 15, faculty: "Математический" },
    { name: "Аудиторя 17", places: 20, faculty: "Математический" },
    { name: "Налоговая комната", places: 18, faculty: "Экономический" },
    { name: "Аудиторя 9", places: 17, faculty: "Экономический" },
    { name: "Аудиторя 10", places: 15, faculty: "Экономический" }
]

academy.printAll = function() {
    this.forEach(function(item){
       document.write(item.name + " --- Факультет:  " + item.faculty + " --- Мест: " + item.places + "<br/>"); 
    });
}

academy.printForFacylty = function(facylty) {
    this.forEach(function(item){
        if(item.faculty == facylty)
       document.write(item.name + " --- Факультет:  " + item.faculty + " --- Мест: " + item.places + "<br/>"); 
    });
}

academy.printForGroup = function(group){
     this.forEach(function(item){
        if(item.faculty == group.faculty &&
          item.places >= group.students)
       document.write(item.name + " --- Факультет:  " + item.faculty + " --- Мест: " + item.places + "<br/>"); 
    });
}

academy.sortByPlaces = function(){
    function comparePlace(roomA, roomB){
    return roomA.places - roomB.places;
}
    this.sort(comparePlace);
}

academy.sortByName = function(){
    function compareName(a, b){
     if (a.name > b.name) {
    return 1;
  }
  if (a.name < b.name) {
    return -1;
  }
  // a должно быть равным b
  return 0;
}
    this.sort(compareName);
}


var group =  { name: "МС-03-1д", students: 20, faculty: "Металлургический" };
var group2 =  { name: "ТМ-03-2д", students: 12, faculty: "Математический" };

academy.printAll();
document.write("<br/>");
academy.printForFacylty("Экономический");
document.write("<br/>");
academy.printForGroup(group);
document.write("<br/>");
academy.printForGroup(group2);

document.write("<br/>");
document.write("<br/>");

academy.sortByPlaces();
academy.printAll();

document.write("<br/>");
document.write("<br/>");

academy.sortByName();
academy.printAll();
