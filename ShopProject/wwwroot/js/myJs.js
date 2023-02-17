

function addItem() {

    var ul = document.getElementById("dynamic-list");
    var candidate = document.getElementById("candidate");

    var allInputs = document.getElementsByName("propertyName");


    var e = document.getElementById("errorInputProperty");

    for (let str of allInputs) {
        if (candidate.value == str.value) {
            candidate.style.borderColor = "red";

            if (e == null) {
                var e = document.createElement('div'); e.setAttribute("id", "errorInputProperty"); e.innerHTML = "this property u added"
                ul.appendChild(e);
            }


            return;
        }

    }
    if (e != null) {
        ul.removeChild(e)
        candidate.style.borderColor = "black";
    }


    var div = document.createElement("div"); div.setAttribute("id", candidate.value)
    div.innerHTML = `<h5>enter name</h5> <input name="propertyName" value=${candidate.value}> <input id=${candidate.value + 1} type="hidden" name="propertyType" value=1>`
    div.innerHTML += `<select onchange="changeSelect(this)"> <option value=1>целочисленное</option>   <option value=2>точное</option><option value="3">строковое</option> <option value="4">флаг</option> <option value="5">дата</option> </select><hr/>`

    //div.innerHTML = `<h2>Hello</h2>`;

    ul.appendChild(div);

}




function removeItem() {
    var ul = document.getElementById("dynamic-list");
    var candidate = document.getElementById("candidate");
    var item = document.getElementById(candidate.value);
    ul.removeChild(item);
}

function changeSelect(param) {

    var hiddenElem = param.previousElementSibling;
    hiddenElem.value = param.value;

}
