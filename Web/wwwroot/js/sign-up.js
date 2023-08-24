let usernameInput = document.querySelector(".username-input")
let emailInput = document.querySelector(".email-input")
let passwordInput = document.querySelector(".password-input")
let confirmPasswordInput = document.querySelector(".confirm-password-input")
let submitBtn = document.querySelector(".sign-up-button-disable")

confirmPasswordInput.oninput = () =>
{
    if (confirmPasswordInput.value != passwordInput.value)
    {
        confirmPasswordInput.className = "confirm-password-input input-element-warning"
    }
    else
    {
        confirmPasswordInput.className = "confirm-password-input input-element"
    }
}

submitBtn.onclick = () =>
{
    let newUser =
    {
        username: usernameInput.value,
        email: emailInput.value,
        password: passwordInput.value
    }

    fetch(`${window.location.origin}/api/session/signup`,{
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(newUser)
    })
    .then((response) => {
        if (response.ok)
        {
            window.location.replace(`${document.location.origin}/home/signin`);
        }
        return response.json();
    })
}