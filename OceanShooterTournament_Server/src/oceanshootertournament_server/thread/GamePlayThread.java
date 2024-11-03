/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package oceanshootertournament_server.thread;

import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Collections;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;
import java.util.Queue;
import java.util.Random;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.Executors;
import java.util.concurrent.ScheduledExecutorService;
import java.util.concurrent.TimeUnit;
import java.util.logging.Level;
import java.util.logging.Logger;

import oceanshootertournament_server.OceanShooterTournament_Server;
import oceanshootertournament_server.dao.BulletDAO;
import oceanshootertournament_server.dao.FishDAO;
import oceanshootertournament_server.dao.RoomDAO;
import oceanshootertournament_server.dao.UserDAO;
import oceanshootertournament_server.model.*;
import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;
import org.json.simple.parser.ParseException;

/**
 * @author ADMIN
 */
public class GamePlayThread extends Thread {

    public static float widthScene = 11f;
    public static float heightScene = 6f;
    public static int totalFish = 27;
    public static int randomCountFish = 12;
    public static long timeGenerate = 30;
    public static int timeGame = 20;
    public static float damage = 1;

    private String roomID;

    private BulletDAO bulletDAO;
    private FishDAO fishDAO;

    JSONArray fishsData;

    public GamePlayThread(String roomID) {
        this.roomID = roomID;
        bulletDAO = new BulletDAO();
        fishDAO = new FishDAO();
        LoadFishsData();
        startCountdown(timeGame);
    }

    @Override
    public void run() {
        while (true) {
            MoveAll();
            CheckCollision();
            try {
                Thread.sleep(10);
            } catch (InterruptedException ex) {
                Logger.getLogger(GamePlayThread.class.getName()).log(Level.SEVERE, null, ex);
            }
        }
    }

    public void MoveAll() {
        if (bulletDAO.IsEmpty() && fishDAO.IsEmpty()) {
            return;
        }
        JSONObject jsonResponseFrame = new JSONObject();
        jsonResponseFrame.put("idRole", roomID);
        jsonResponseFrame.put("action", "update_new_frame");
        JSONArray dataArrBullet = new JSONArray();
        for (Bullet bullet : bulletDAO.listBullets()) {
            bullet.MoveOneFrame();
            if (bullet.IsBoundary(widthScene, heightScene)) {
                dataArrBullet.add(bullet.GetDataFrame());
            } else {
                DestroyBullet(bullet);
            }
        }
        JSONArray dataArrFish = new JSONArray();
        for (Fish fish : fishDAO.listFishs()) {
            fish.MoveOneFrame();
            if (fish.IsBoundary(widthScene, heightScene)) {
                dataArrFish.add(fish.GetDataFrame());
            } else {
                DestroyFish(fish);
            }
        }

        JSONObject jsonResponseData = new JSONObject();
        jsonResponseData.put("bullets", dataArrBullet);
        jsonResponseData.put("fishs", dataArrFish);
        jsonResponseFrame.put("data", jsonResponseData);
        String mess = jsonResponseFrame.toString();
        SendFrameForAll(mess);
    }

    public void DestroyFish(Fish fish) {
        fishDAO.RemoveDictBullet(fish.getFishID());
    }

    public void DestroyBullet(Bullet bullet) {
        bulletDAO.RemoveDictBullet(bullet.getBulletID());
    }

    public void ResponseBulletInit(Bullet bullet) {
        bulletDAO.AddDictBullet(bullet);
    }

    public void SendFrameForAll(String mess) {
        Room room = RoomDAO.GetRoom(roomID);
        for (int i = 0; i < room.getPlayers().size(); i++) {
            User user = room.getPlayers().get(i);
            OceanShooterTournament_Server.SendResponse(mess, user.getAddress(), user.getPort());
        }
    }

    public void LoadFishsData() {
        JSONParser jsonParser = new JSONParser();
        try (FileReader reader = new FileReader("src/oceanshootertournament_server/data/fishs.json")) {
            // Đọc file JSON
            JSONObject obj = (JSONObject) jsonParser.parse(reader);
            fishsData = (JSONArray) obj.get("fishsData");
            PrintFishData();

        } catch (IOException | ParseException e) {
            e.printStackTrace();
        }
    }

    public void PrintFishData() {
        for (int i = 0; i < fishsData.size(); i++) {
            JSONObject fishData = (JSONObject) fishsData.get(i);
            String result = fishData.get("name") + " " + fishData.get("hp");
            System.out.println(result);
        }
    }

    public List<Integer> RandomFish() {
        List<Integer> numbers = new ArrayList<>();
        for (int i = 0; i < totalFish; i++) {
            numbers.add(i);
        }

        // Xáo trộn danh sách
        Collections.shuffle(numbers);

        // Lấy 12 giá trị đầu tiên
        List<Integer> randomValues = numbers.subList(0, randomCountFish);
        return randomValues;
    }

    public void GenerateFish() {
        List<Integer> randomFishIndex = RandomFish();
        for (int i = 0; i < randomFishIndex.size(); i++) {
            int type = randomFishIndex.get(i);
            JSONObject fishData = (JSONObject) fishsData.get(type);
            float widthFish = (float) ((double) fishData.get("widthFish"));
            float heightFish = (float) ((double) fishData.get("heightFish"));
            float hp = (float) ((double) fishData.get("hp"));
            int coin = (int) ((double) fishData.get("coin"));
            float velocity = RandomVelocityFish();
            int trajectory = RandomTrajectoryFish();
            var directionX = new Random().nextInt(2) == 0 ? -1 : 1;
            var posX = directionX * -widthScene;
            Fish fish = new Fish(posX, widthFish, heightFish, type, hp, trajectory, velocity, directionX, coin);
            fishDAO.AddDictFish(fish);
        }
    }

    public float RandomVelocityFish() {
        Random random = new Random();
        return random.nextInt(10) * 0.1f + 1;
    }

    public int RandomTrajectoryFish() {
        Random random = new Random();
        return random.nextInt(8);
    }

    public void CheckCollision() {
        for (int i = 0; i < bulletDAO.listBullets().size(); i++) {
            Bullet bullet = bulletDAO.listBullets().get(i);
            if (!bulletDAO.checkInDict(bullet.getBulletID())) {
                continue;
            }
            for (int j = 0; j < fishDAO.listFishs().size(); j++) {
                Fish fish = fishDAO.listFishs().get(j);
                if (!fishDAO.checkInDict(fish.getFishID())) {
                    continue;
                }
                if (fish.containsPoint(bullet.getPosition().x, bullet.getPosition().y)) {
                    System.out.println(bullet.getBulletID() + " ban trung " + fish.getFishID());
                    DestroyBullet(bullet);
                    fish.Damaged(damage);
                    if (fish.CheckDie()) {
                        ReceiveCoin(UserDAO.GetUser(bullet.getUserID()), fish.getCoin(), fish.getPosition().x, fish.getPosition().y);
                        DestroyFish(fish);
                    }
                }
            }
        }
    }

    public void ReceiveCoin(User user, int coinReceive, float posX, float posY) {
        user.ReceiveCoin(coinReceive);
        JSONObject jsonResponse = new JSONObject();
        jsonResponse.put("idRole", roomID);
        jsonResponse.put("action", "receive_coin");

        JSONObject dataCoin = new JSONObject();
        dataCoin.put("userID", user.getUserID());
        dataCoin.put("coinReceive", coinReceive);
        dataCoin.put("coinTotal", user.getCoin());
        dataCoin.put("posX", posX);
        dataCoin.put("posY", posY);

        jsonResponse.put("data", dataCoin);
        String messDestroy = jsonResponse.toString();
        SendFrameForAll(messDestroy);
    }

    public void ChangeBet(String userID, int indexBet) {
        JSONObject jsonResponse = new JSONObject();
        jsonResponse.put("idRole", roomID);
        jsonResponse.put("action", "change_bet");

        JSONObject dataJson = new JSONObject();
        dataJson.put("userID", userID);
        dataJson.put("index", indexBet);

        jsonResponse.put("data", dataJson);
        String mess = jsonResponse.toString();
        Room room = RoomDAO.GetRoom(roomID);
        for (int i = 0; i < room.getPlayers().size(); i++) {
            User user = room.getPlayers().get(i);
            if (user.getUserID().equals(userID)) {
                continue;
            }
            OceanShooterTournament_Server.SendResponse(mess, user.getAddress(), user.getPort());
        }
    }

    public void startCountdown(int seconds) {
        ScheduledExecutorService scheduler = Executors.newScheduledThreadPool(1);

        Runnable countdownTask = new Runnable() {
            int timeLeft = seconds;

            @Override
            public void run() {
                if (timeLeft > 0) {
                    System.out.println("Thời gian còn lại: " + timeLeft + " giây");
                    timeLeft--;
                    SendTimeToUser(timeLeft);
                } else {
                    System.out.println("Hết giờ");                    
                    scheduler.shutdown();
                    EndGame();
                }
            }
        };
        
        Runnable generateFish = () -> {
            GenerateFish();
        };

        // Schedule the task to run every second
        scheduler.scheduleAtFixedRate(countdownTask, 4, 1, TimeUnit.SECONDS);
        scheduler.scheduleAtFixedRate(generateFish, 3, timeGenerate, TimeUnit.SECONDS);
    }

    public void SendTimeToUser(int time) {
        JSONObject jsonResponse = new JSONObject();
        jsonResponse.put("idRole", roomID);
        jsonResponse.put("action", "update_timer");
        jsonResponse.put("data", time);
        String mess = jsonResponse.toString();
        SendFrameForAll(mess);
    }

    public void EndGame() {
        DestroyAll();
        JSONObject jsonResponse = new JSONObject();
        jsonResponse.put("idRole", roomID);
        jsonResponse.put("action", "end_game");
        jsonResponse.put("data", RoomDAO.GetRoom(roomID).GetDataEndGame());
        String mess = jsonResponse.toString();
        SendFrameForAll(mess);
    }
    
    public void DestroyAll() {
        JSONObject jsonResponseFrame = new JSONObject();
        jsonResponseFrame.put("idRole", roomID);
        jsonResponseFrame.put("action", "update_new_frame");
        JSONArray dataArrBullet = new JSONArray();
        for (Bullet bullet : bulletDAO.listBullets()) {
            DestroyBullet(bullet);
        }
        JSONArray dataArrFish = new JSONArray();
        for (Fish fish : fishDAO.listFishs()) {
            DestroyFish(fish);
        }

        JSONObject jsonResponseData = new JSONObject();
        jsonResponseData.put("bullets", dataArrBullet);
        jsonResponseData.put("fishs", dataArrFish);
        jsonResponseFrame.put("data", jsonResponseData);
        String mess = jsonResponseFrame.toString();
        SendFrameForAll(mess);
    }
}
