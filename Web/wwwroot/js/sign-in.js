let emailInput = document.querySelector(".email-input")
let passwordInput = document.querySelector(".password-input")
let submitBtn = document.querySelector(".sign-in-button-disable")

submitBtn.onclick = () =>
{
    let user =
    {
        login: emailInput.value,
        password: passwordInput.value
    }
    
    fetch(`${window.location.origin}/api/session/signin`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(user)
    })
    .then((response) => {
        if (response.ok)
        {
            return response.json();
        }
        else
        {
            //some logic
        }        
    })
    .then((result) => {
        document.cookie = `jwt=${result.token};path=/;`
        //cookie.setItem("jwt", result.token)
        window.location.replace(`${document.location.origin}/list/mylists`)
    })
}