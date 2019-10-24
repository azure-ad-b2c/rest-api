const express = require('express')
var bodyParser = require('body-parser')
const app = express()
const port = 3000
const appVersion = '1.0.0'

// parse application/json
app.use(bodyParser.json())


// Welcome message
app.get('/', (req, res) => res.send('Welcome to Azure AD B2C custom REST API'))

app.post('/api/identity/loyalty', function (req, res) {

    //Check if the language parameter is provided
    if (!req.body.language) {
        res.status(409).json({ version: appVersion, status: 409, userMessage: 'Language code is null or empty' });
        return;
    }

    //Check if the objectId parameter is provided
    if (!req.body.objectId) {
        res.status(409).json({ version: appVersion, status: 409, userMessage: 'User object Id is null or empty' });
        return;
    }

    // Return the loaylty number
    res.json({
        loyaltyNumber: req.body.language + "-" + (Math.floor(Math.random() * 9999) + 1000)
    });

    res.json({ user: 'tobi' });
})

app.post('/api/identity/validate', function (req, res) {

    //Check if the language parameter is provided
    if (!req.body.language) {
        res.status(409).json({ version: appVersion, status: 409, userMessage: 'Language code is null or empty' });
        return;
    }

    //Check if the email parameter is provided
    if (!req.body.email) {
        res.status(409).json({ version: appVersion, status: 409, userMessage: 'Email is null or empty' });
        return;
    }

    // Check if the email parameter starts with 'test'
    if (req.body.email.toLowerCase().startsWith("test")) {
        res.status(409).json({ version: appVersion, status: 409, userMessage: "Your email address cannot start with 'test'" });
        return;
    }

    // Return the loyalty number and email in lower case
    res.json({
        loyaltyNumber: req.body.language + "-" + (Math.floor(Math.random() * 9999) + 1000),
        email: req.body.email.toLowerCase()
    });
})

app.listen(port, () => console.log(`Example app listening on port ${port}!`))