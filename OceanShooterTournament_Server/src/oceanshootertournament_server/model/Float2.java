package oceanshootertournament_server.model;

import org.json.simple.JSONObject;

public class Float2 {
    public float x;
    public float y;

    public Float2(float x, float y) {
        this.x = x;
        this.y = y;
    }
    
    public Float2 Normalize() {
        float length = (float) Math.sqrt(x * x + y * y);
        return new Float2(x / length, y / length);
    }
    
    public Float2 Add(Float2 other) {
        return new Float2(x + other.x, y + other.y);
    }
    
    public Float2 Mul(float value) {
        return new Float2(x * value, y * value);
    }
    
    public JSONObject GetData() {
        JSONObject jsonResponse = new JSONObject();
        jsonResponse.put("x", x);
        jsonResponse.put("y", y);
        return jsonResponse;
    }
}
