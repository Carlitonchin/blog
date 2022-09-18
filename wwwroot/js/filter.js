var titles = document.getElementsByClassName("title-note");
var bodies = document.getElementsByClassName("body-note");

function filter_title(text){
    for(let i = 0; i < titles.length; i++)
    {
        let title = titles[i]
        let parent = title.parentNode.parentNode.parentNode;
        let ocurr = title.innerHTML.toLowerCase().includes(text.toLowerCase())

        parent.style.display = ocurr ? "block":"none";
    }
    
}

function filter_body(text){
    for(let i = 0; i < bodies.length; i++)
    {
        let body = bodies[i]
        let parent = body.parentNode.parentNode;
        let ocurr = body.innerHTML.toLowerCase().includes(text.toLowerCase())

        parent.style.display = ocurr ? "block":"none";
    }
}

var title_filter = document.getElementById("title_filter");
var body_filter = document.getElementById("body_filter");

title_filter.addEventListener("input", e=>filter_title(e.target.value))
body_filter.addEventListener("input", e=>filter_body(e.target.value))