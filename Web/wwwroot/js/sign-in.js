let emailInput = document.querySelector(".email-input")
let passwordInput = document.querySelector(".password-input")
let submitBtn = document.getElementById("sign-in-button")

emailInput.addEventListener("input", DataInput.bind(emailInput, emailInput, passwordInput, submitBtn, "email"))
passwordInput.addEventListener("input", DataInput.bind(passwordInput, emailInput, passwordInput, submitBtn, "password"))

function DataInput(emailInput, passwordInput, btn, from)
{
    if (from === "email")
    {
        emailInput.className = "email-input input-element"
    }
    else
    {
        passwordInput.className = "password-input input-element"
    }

    if (emailInput.Value === "" || passwordInput.value === "")
    {
        btn.className = "sign-in-button-disable input-element"
        btn.addEventListener("click", SignIn.bind(submitBtn, emailInput, passwordInput))
    }
    else
    {
        btn.className = "sign-in-button-enable input-element"
        btn.addEventListener("click", () => {});
    }
}

function SignIn(emailInput, passwordInput)
{    
    let user =
    {
        email: emailInput.value,
        password: passwordInput.value
    }
    console.log(user)
    
    fetch(`${window.location.origin}/api/session/signin`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(user)
    })
    .then((response) => {
        return response.json();    
    })
    .then((result) => {
        if (result.code == 40401)
        {
            emailInput.className = "email-input input-element-warning"
        }
        
        if (result.code == 40301)
        {
            passwordInput.className = "password-input input-element-warning"
        }

        if (result.token)
        {
            document.cookie = `jwt=${result.token};path=/;`
            window.location.replace(`${document.location.origin}/list/mylists`)
        }
    })
}