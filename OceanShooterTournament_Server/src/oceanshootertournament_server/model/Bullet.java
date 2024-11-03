/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package oceanshootertournament_server.model;

import java.net.InetAddress;

import org.json.simple.JSONObject;

/**
 * @author ADMIN
 */
public class Bullet {

    private Float2 position;
    private Float2 velocity;
    String bulletID;
    private String userID;
    private int type;

    public Bullet() {
    }

    public Bullet(Float2 position, Float2 velocity, String bulletID, String userID, int type) {
        this.position = position;
        this.velocity = velocity;
        this.bulletID = bulletID;
        this.userID = userID;
        this.type = type;
    }

    public Float2 getPosition() {
        return position;
    }

    public void setPosition(Float2 position) {
        this.position = position;
    }

    public Float2 getVelocity() {
        return velocity;
    }

    public void setVelocity(Float2 velocity) {
        this.velocity = velocity;
    }

    public String getBulletID() {
        return bulletID;
    }

    public void setBulletID(String bulletID) {
        this.bulletID = bulletID;
    }

    public String getUserID() {
        return userID;
    }

    public void setUserID(String userID) {
        this.userID = userID;
    }

    public JSONObject GetDataFrame() {
        JSONObject jsonResponseData = new JSONObject();
        jsonResponseData.put("itemID", getBulletID());
        jsonResponseData.put("itemType", type);
        jsonResponseData.put("position", position.GetData());
        jsonResponseData.put("velocity", velocity.GetData());
        jsonResponseData.put("userID", userID);
        return jsonResponseData;
    }

    public void MoveOneFrame() {
        position = position.Add(velocity.Mul(0.1f));
    }

    public boolean IsBoundary(float width, float height) {
        return Math.abs(position.x) <= width && Math.abs(position.y) <= height;
    }
}
