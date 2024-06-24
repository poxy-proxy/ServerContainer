const express = require("express");
const knex = require("knex");

const db = knex({ 
    client: "pg",
    connection: {
        host: "172.19.2.63",
        port: "5432",
        user: "Anton@example.com",
        password: "X@lodilnik97!!!!!!!",
        database: "antondb"
    }
});


const app = express();

app.use(async (req, res) => {
    const result = await db("users");
    res.send(JSON.stringify(result));
})

app.listen(3000, () => console.log("Server is running on port 3000"))