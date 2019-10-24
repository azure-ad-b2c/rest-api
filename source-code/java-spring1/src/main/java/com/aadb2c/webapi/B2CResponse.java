package com.aadb2c.webapi;

public class B2CResponse extends B2CResponseModel
{
    private String loyaltyNumber;
    private String email;

    
    
    // User message
    public String getLoyaltyNumber() {
        return loyaltyNumber;
    }

    public void setLoyaltyNumber(String loyaltyNumber) {
        this.loyaltyNumber = loyaltyNumber;
    }

    
    // User message
    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }
    
    public B2CResponse(String loyaltyNumber, String email) {
        this.setStatus(200);
        this.setLoyaltyNumber(loyaltyNumber);
        this.setEmail(email);
    }
}