var titles = document.getElementsByClassName("title-note");
var bodies = document.getElementsByClassName("body-note");

var title_filter = document.getElementById("title_filter");
var body_filter = document.getElementById("body_filter");

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

function filter(){
    for(let i = 0; i < titles.length; i++)
    {
        let title = titles[i]
        let body = bodies[i]

        let text_title = title_filter.value
        let text_body = body_filter.value

        let parent = title.parentNode.parentNode.parentNode;

        let ocurr_title = title.innerHTML.toLowerCase().includes(text_title.toLowerCase())
        let ocurr_body = body.innerHTML.toLowerCase().includes(text_body.toLowerCase())

        parent.style.display = ocurr_title && ocurr_body ? "block":"none";
    }

    
}

title_filter.addEventListener("input", filter)
body_filter.addEventListener("input", filter)