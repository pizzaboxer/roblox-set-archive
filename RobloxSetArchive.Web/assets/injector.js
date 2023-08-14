// https://stackoverflow.com/a/7053197/11852173

function ready(callback){
    // in case the document is already rendered
    if (document.readyState!='loading') callback();
    // modern browsers
    else if (document.addEventListener) document.addEventListener('DOMContentLoaded', callback);
    // IE <= 8
    else document.attachEvent('onreadystatechange', function(){
        if (document.readyState=='complete') callback();
    });
}

ready(function(){
    // https://stackoverflow.com/a/11371599/11852173
    var css = '.nav-link-image { opacity: .5; } .nav-link-image:hover { opacity: 1; }';

    var style = document.createElement('style');

    if (style.styleSheet) {
        style.styleSheet.cssText = css;
    } else {
        style.appendChild(document.createTextNode(css));
    }

    document.getElementsByTagName('head')[0].appendChild(style);

    var container = document.getElementsByClassName("navbar-nav ms-auto")[0];

    container.innerHTML += 
        '<li class="nav-item">' + 
            '<a class="nav-link nav-link-image py-0" href="https://github.com/pizzaboxer/roblox-set-archive">' +
                '<img src="/img/github.png" width="24">' +
            '</a>' +
        '</li>' +
        '<li class="nav-item">' + 
            '<a class="nav-link nav-link-image py-0" href="https://devforum.roblox.com/t/roblox-set-archive-an-archive-of-user-created-sets-from-2010-to-2018/2209165">' +
                '<img src="/img/devforum.svg" width="24">' +
            '</a>' +
        '</li>';
});