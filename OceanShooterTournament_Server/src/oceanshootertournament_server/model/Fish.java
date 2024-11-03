/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package oceanshootertournament_server.model;

import java.util.ArrayList;
import java.util.List;
import java.util.Random;
import java.util.function.Function;

import org.json.simple.JSONObject;

/**
 * @author pc
 */
public class Fish {

    private String fishID;
    private Float2 position;
    private Float2 direction;
    private float velocity;
    private float angle;
    private int directionX;
    private float widthFish;
    private float heightFish;
    private int type;
    private float hp;
    private int indexTrajectory;
    private int coin;

    List<Function<Float, Float>> listTrajectory;
    List<Function<Float, Float>> listDerivative;

    public Fish() {
    }

    public Fish(float x, float widthFish, float heightFish, int type, float hp, int indexTrajectory, float velocity, int directionX, int coin) {
        try {
            InitTrajectory();
            InitDerivative();
            this.position = new Float2(0, 0);
            this.position.x = x;
            this.widthFish = widthFish;
            this.heightFish = heightFish;
            this.type = type;
            this.hp = hp;
            this.velocity = velocity;
            this.directionX = directionX;
            this.indexTrajectory = indexTrajectory;
            this.coin = coin;
            SetNewDirection();

            // Kiểm tra xem indexTrajectory có hợp lệ không
            if (indexTrajectory < 0 || indexTrajectory >= listTrajectory.size()) {
                throw new IndexOutOfBoundsException("indexTrajectory is out of bounds: " + indexTrajectory);
            }

            this.position.y = listTrajectory.get(indexTrajectory).apply(position.x);
        } catch (Exception e) {
            e.printStackTrace();
            // Thêm log nếu cần thiết
        }
    }

    public void InitTrajectory() {
        listTrajectory = new ArrayList<>();
        listTrajectory.add(x -> (0.4f * x + 1));
        listTrajectory.add(x -> (-0.05f * x * x + 0.3f * x + 2));
        listTrajectory.add(x -> (0.02f * x * x * x - 0.1f * x * x + 0.5f * x - 1));
        listTrajectory.add(x -> (0.1f * x * x - 2));
        listTrajectory.add(x -> (-0.1f * x + 3));
        listTrajectory.add(x -> ((float) Math.cos(x * 0.5f) * 2));
        listTrajectory.add(x -> (0.01f * x * x * x + 0.2f * x * x - 0.3f * x + 1));
        listTrajectory.add(x -> (-0.02f * x * x + x - 2));
    }

    public void InitDerivative() {
        listDerivative = new ArrayList<>();
        listDerivative.add(x -> 0.4f);  // Đạo hàm của 0.4f * x + 1
        listDerivative.add(x -> -0.1f * x + 0.3f);  // Đạo hàm của -0.05f * x^2 + 0.3f * x + 2
        listDerivative.add(x -> 0.06f * x * x - 0.2f * x + 0.5f);  // Đạo hàm của 0.02f * x^3 - 0.1f * x^2 + 0.5f * x - 1
        listDerivative.add(x -> 0.2f * x);  // Đạo hàm của 0.1f * x^2 - 2
        listDerivative.add(x -> -0.1f);  // Đạo hàm của -0.1f * x + 3
        listDerivative.add(x -> (float) (-Math.sin(x * 0.5f) * 0.5f * 2));  // Đạo hàm của cos(x * 0.5f) * 2
        listDerivative.add(x -> 0.03f * x * x + 0.4f * x - 0.3f);  // Đạo hàm của 0.01f * x^3 + 0.2f * x^2 - 0.3f * x + 1
        listDerivative.add(x -> -0.04f * x + 1);  // Đạo hàm của -0.02f * x^2 + x - 2
    }

    public int getCoin() {
        return coin;
    }

    public void setCoin(int coin) {
        this.coin = coin;
    }

    public String getFishID() {
        return fishID;
    }

    public void setFishID(String fishID) {
        this.fishID = fishID;
    }

    public Float2 getPosition() {
        return position;
    }

    public void setPosition(Float2 position) {
        this.position = position;
    }

    public float getWidthFish() {
        return widthFish;
    }

    public void setWidthFish(float widthFish) {
        this.widthFish = widthFish;
    }

    public float getHeightFish() {
        return heightFish;
    }

    public void setHeightFish(float heightFish) {
        this.heightFish = heightFish;
    }

    public int getType() {
        return type;
    }

    public void setType(int type) {
        this.type = type;
    }

    public float getHp() {
        return hp;
    }

    public void setHp(float hp) {
        this.hp = hp;
    }

    public int getIndexTrajectory() {
        return indexTrajectory;
    }

    public void setIndexTrajectory(int indexTrajectory) {
        this.indexTrajectory = indexTrajectory;
    }

    public float getVelocity() {
        return velocity;
    }

    public void setVelocity(float velocity) {
        this.velocity = velocity;
    }

    public float getAngle() {
        return angle;
    }

    public void setAngle(float angle) {
        this.angle = angle;
    }


    public List<Function<Float, Float>> getListTrajectory() {
        return listTrajectory;
    }

    public void setListTrajectory(List<Function<Float, Float>> listTrajectory) {
        this.listTrajectory = listTrajectory;
    }

    public Float2 getDirection() {
        return direction;
    }

    public void setDirection(Float2 direction) {
        this.direction = direction;
    }

    public int getDirectionX() {
        return directionX;
    }

    public void setDirectionX(int directionX) {
        this.directionX = directionX;
    }

    public void MoveOneFrame() {
        position.x += velocity * directionX / 100;
        position.y = listTrajectory.get(indexTrajectory).apply(position.x);
        SetNewDirection();
    }

    private void SetNewDirection() {
        float slope = listDerivative.get(indexTrajectory).apply(position.x);
        double angleInRadians = Math.atan(slope);
        this.direction = new Float2((float) Math.cos(angleInRadians), (float) Math.sin(angleInRadians)).Normalize();
        this.angle = (float) Math.toDegrees(angleInRadians);
    }

    public boolean IsBoundary(float width, float height) {
        return Math.abs(position.x) <= width;
    }

    public JSONObject GetDataFrame() {
        JSONObject jsonResponseData = new JSONObject();
        jsonResponseData.put("itemID", getFishID());
        jsonResponseData.put("itemType", getType());
        jsonResponseData.put("position", position.GetData());
        jsonResponseData.put("velocity", direction.Mul(directionX).GetData());
        return jsonResponseData;
    }

    public boolean containsPoint(float px, float py) {
        // Chuyển đổi điểm về hệ tọa độ của hình chữ nhật đã quay
        double cosTheta = Math.cos(Math.toRadians(angle));
        double sinTheta = Math.sin(Math.toRadians(angle));

        // Tọa độ điểm p trong hệ tọa độ của hình chữ nhật
        double localX = cosTheta * (px - position.x) + sinTheta * (py - position.y);
        double localY = -sinTheta * (px - position.x) + cosTheta * (py - position.y);

        // Kiểm tra nếu tọa độ nằm trong chiều dài và chiều rộng của hình chữ nhật
        return Math.abs(localX) <= widthFish && Math.abs(localY) <= heightFish;
    }

    public void Damaged(float damage) {
        hp -= damage;
    }

    public boolean CheckDie() {
        return hp <= 0;
    }

}
