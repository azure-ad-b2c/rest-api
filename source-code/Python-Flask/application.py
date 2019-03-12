from flask import Flask, jsonify, request, Response
import json
import random
app = Flask(__name__)

@app.route("/")
def home():
    return "Welcome to Azure AD B2C custom REST API"

@app.route("/api/identity/loyalty", methods=['POST'])
def loyalty():

    # Check if the request.data is provided
    if not (request.data):
        return Response(json.dumps({"status": 409, "version": "1.0.0", "userMessage": "Request content is null" }), status=409, mimetype="application/json")

    # Try to deserialize the JSON data
    try:
        request_data = request.get_json()
    except:
        return Response(json.dumps({"status": 409, "version": "1.0.0", "userMessage": "Invalid JSON data"}), status=409, mimetype="application/json")

    # Check if the language parameter is provided
    if not ("language" in request_data):
        return Response(json.dumps({"status": 409, "version": "1.0.0", "userMessage": "Language code is null or empty" }), status=409, mimetype="application/json")

    # Check if the objectId parameter is provided
    if not ("objectId" in request_data):
        return Response(json.dumps({"status": 409, "version": "1.0.0", "userMessage": "User object Id is null or empty" }), status=409, mimetype="application/json")

    # Return the loyalty number
    return jsonify({"loyaltyNumber": request_data["language"] + "-" + str(random.randint(1000,9999)) })


@app.route("/api/identity/validate", methods=['POST'])
def validate():

    # Check if the request.data is provided
    if not (request.data):
        return Response(json.dumps({"status": 409, "version": "1.0.0", "userMessage": "Request content is null" }), status=409, mimetype="application/json")

    # Try to deserialize the JSON data
    try:
        request_data = request.get_json()
    except:
        return Response(json.dumps({"status": 409, "version": "1.0.0", "userMessage": "Invalid JSON data"}), status=409, mimetype="application/json")

    # Check if the language parameter is provided
    if not ("language" in request_data):
        return Response(json.dumps({"status": 409, "version": "1.0.0", "userMessage": "Language code is null or empty" }), status=409, mimetype="application/json")

    # Check if the email parameter is provided
    if not ("email" in request_data):
        return Response(json.dumps({"status": 409, "version": "1.0.0", "userMessage": "Email is null or empty" }), status=409, mimetype="application/json")

    # Check if the email parameter starts with 'test'
    if str.startswith(request_data["email"].lower(), "test"):
        return Response(json.dumps({"status": 409, "version": "1.0.0", "userMessage": "Your email address cannot start with 'test'" }), status=409, mimetype="application/json")

    # Return the loyalty number
    return jsonify(
        {
            "loyaltyNumber": request_data["language"] + "-" + str(random.randint(1000,9999)),
            "email": request_data["email"].lower()
        })
