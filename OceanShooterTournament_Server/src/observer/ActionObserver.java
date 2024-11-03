/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Interface.java to edit this template
 */
package observer;

import java.net.DatagramPacket;

/**
 *
 * @author pc
 */
public interface ActionObserver {
    void executeAction(DatagramPacket receivePacket);
}
