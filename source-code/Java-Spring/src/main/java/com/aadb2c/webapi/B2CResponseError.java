package com.aadb2c.webapi;

public class B2CResponseError extends B2CResponseModel
{
    public B2CResponseError(String userMessage) {
        this.setStatus(409);
        this.setUserMessage(userMessage);
    }

}