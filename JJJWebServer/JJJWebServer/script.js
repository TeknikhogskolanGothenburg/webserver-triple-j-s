function pandaClick() {
    var pandaImageElement = document.getElementById("PandaImage");
    if(pandaImageElement.style.visibility === "hidden" ){
        pandaImageElement.style.visibility = "visible" ;
        document.getElementById("PandaButton").innerHTML = "Hide panda";
        
    }else{
        pandaImageElement.style.visibility = "hidden" ;
        document.getElementById("PandaButton").innerHTML = "What does the panda think?";
    }
}