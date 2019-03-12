package com.aadb2c.webapi;

import java.util.Map;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class IdentityController {
    @GetMapping("/")
    public String welcome() {
        return "Welcome to Azure AD B2C custom REST API";
    }

    @PostMapping("/api/identity/loyalty")
    public ResponseEntity<B2CResponseModel> loyalty(@RequestBody(required = false) Map<String, String> body) {

        // Check if the POST data is provided
        if (body == null || body.isEmpty()) {
            return ResponseEntity.status(HttpStatus.CONFLICT).body(new B2CResponseError("Request content is null"));
        }

        // Check if the language parameter is provided
        if (body.containsKey("language") == false || body.get("language").isEmpty()) {
            return ResponseEntity.status(HttpStatus.CONFLICT)
                    .body(new B2CResponseError("Language code is null or empty"));
        }

        // Check if the objectId parameter is provided
        if (body.containsKey("objectId") == false || body.get("objectId").isEmpty()) {
            return ResponseEntity.status(HttpStatus.CONFLICT)
                    .body(new B2CResponseError("User object Id is null or empty"));
        }

        return ResponseEntity
                .ok(new B2CResponse(body.get("language") + "-" + ((int) (Math.random() * 9999 + 1000)), null));
    }

    @PostMapping("/api/identity/validate")
    public ResponseEntity<B2CResponseModel> validate(@RequestBody(required = false) Map<String, String> body) {

        // Check if the POST data is provided
        if (body == null || body.isEmpty()) {
            return ResponseEntity.status(HttpStatus.CONFLICT).body(new B2CResponseError("Request content is null"));
        }

        // Check if the language parameter is provided
        if (body.containsKey("language") == false || body.get("language").isEmpty()) {
            return ResponseEntity.status(HttpStatus.CONFLICT)
                    .body(new B2CResponseError("Language code is null or empty"));
        }

        // Check if the email parameter is provided
        if (body.containsKey("email") == false || body.get("email").isEmpty()) {
            return ResponseEntity.status(HttpStatus.CONFLICT).body(new B2CResponseError("Email is null or empty"));
        }

        // Check if the email parameter starts with 'test'
        if (body.get("email").toLowerCase().startsWith("test")) {
            return ResponseEntity.status(HttpStatus.CONFLICT)
                    .body(new B2CResponseError("Your email address cannot start with 'test'"));
        }

        return ResponseEntity.ok(new B2CResponse(body.get("language") + "-" + ((int) (Math.random() * 9999 + 1000)),
                body.get("email").toLowerCase()));
    }
}