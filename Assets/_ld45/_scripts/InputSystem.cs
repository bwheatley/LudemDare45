using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


/*
 * This is the input manager that manages all the games inputs
 *
 *
 * TIPS FOR NEW INPUT SYSTEM
 * STARTED = FRAME IS WAS PRESSED
 * PERFORMED = After done holding down
 */
public class InputSystem : MonoBehaviour, InputMaster.IPlayerActions
{


    #region FIELDS

    public static InputSystem instance;
    private InputMaster controls;

    //START CAMERA SETTINGS
    public Camera theCamera;
    public float camera_zoom_speed = 5;
    public int   minZoom           = 1;
    public int   maxZoom           = 50;
    public bool mouseWheelDisabled = false; //If this is set don't touch the mousewheel

    //Screen edge detection
    [HideInInspector]
    public static float top_edge;
    [HideInInspector]
    public static float bottom_edge;
    [HideInInspector]
    public static float left_edge;
    [HideInInspector]
    public static float right_edge;

    //END CAMERA SETTINGS

    //Mouse
    public bool isClicked = false;
    public bool isUnClicked = false;
    public bool isDrag = false;
    public Vector2 mouse;    //Mouse Vector 2 position
    public Vector3 mousePos;    //Mouse Vector3 Fudged

    // public LayerMask defaultMask;

    //Movement
    public bool moveStarted = false;
    private Vector2 movement;

    private Vector3 direction;
    //End Movement

    public static Vector2 lastClickPosition;
    //This is a test remove after you figure it out
    public GameObject player;

    #endregion

    private void Awake() {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        controls                        =  new InputMaster();
        //This makes sure our interface calls back to us here!
        controls.Player.SetCallbacks(this);
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }

    public void OnClick(InputAction.CallbackContext ctx) {
        // Click();

        // if (ctx.performed) {
        //     isClicked = true;
        //     // Debug.Log(string.Format("We clicked at {0}!!!", Mouse.current.position.ReadValue() ));
        // }

        if (ctx.started) {
            isClicked = true;
            // Debug.Log(string.Format("We clicked at {0}!!!", Mouse.current.position.ReadValue() ));
        }

        if (ctx.performed || ctx.canceled) {
            // Debug.Log(string.Format("Click Stopped!!!"));
            isClicked = false;
            isUnClicked = true;
        }
    }

    public void Update() {
        // mouse    = Mouse.current.position.ReadValue();
        // mousePos = new Vector3(mouse.x, mouse.y, 0);

        //Check if we need to move
        if (moveStarted) {
            Move(movement);
        }
    }

    public void ZoomCamera(float zoomValue)
    {
        //Zoom in and out
        // Scroll forward
        if (zoomValue > 0)
        {
            // Debug.Log(string.Format("Zoom In!"));
            ZoomOrthoCamera(theCamera.ScreenToWorldPoint(mousePos), camera_zoom_speed);
            // ZoomOrthoCamera(theCamera.ScreenToWorldPoint(Input.mousePosition), camera_zoom_speed);
        }
        // Scoll back
        if (zoomValue < 0)
        {
            // Debug.Log(string.Format("Zoom Out!"));
            ZoomOrthoCamera(theCamera.ScreenToWorldPoint(mousePos), -camera_zoom_speed);
            // ZoomOrthoCamera(theCamera.ScreenToWorldPoint(Input.mousePosition), -camera_zoom_speed);
        }
    }

    public void ZoomOrthoCamera(Vector3 zoomTowards, float amount)
    {
        // TODO Uncomment this and fix it later
        // if (!MouseInScene(mousePos) || mouseWheelDisabled)
        // {
        //     return;
        // }


        // Calculate how much we will have to move towards the zoomTowards position
        float multiplier = (1.0f / theCamera.orthographicSize * amount);

        //If the size is 1 we don't want to keep moving the mouse, it's annoying as shit
        if (theCamera.orthographicSize > 1 && theCamera.orthographicSize < 50)
        {
            // Move camera
            //TODO make this actually zoom to the mouse
            transform.position += (zoomTowards - transform.position) * multiplier * Time.deltaTime;
            //transform.Translate(transform.position-zoomTowards * Time.deltaTime * multiplier,Space.World);
        }

        // Zoom camera
        theCamera.orthographicSize -= amount;

        // Limit zoom
        theCamera.orthographicSize = Mathf.Clamp(theCamera.orthographicSize, minZoom, maxZoom);

    }

    /// <summary>
    /// Is the mouse inside the bounds of the window
    /// </summary>
    /// <returns></returns>
    public bool MouseInScene(Vector3 MousePosition)
    {
        if (MousePosition.y > top_edge  + 50 || MousePosition.y < bottom_edge - 50 ||
            MousePosition.x < left_edge - 50 || MousePosition.x > right_edge  + 50)
        {
            //Util.WriteDebugLog(string.Format("Mouse out of scene {0} T:{1} B:{2} L:{3} R:{4} ", MousePosition, top_edge, bottom_edge, left_edge, right_edge),GameManager.LogLevel_Debug);
            return false;
        }
        return true;
    }


    /// <summary>
    /// What is the point at which the mouse ray intersects with Y=0
    /// </summary>
    /// <returns></returns>
    public Vector3 HitPos() {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //What is the point at which the mouse ray intersects Y=0

        if (mouseRay.direction.y >= 0)
        {
            // Util.Util.WriteDebugLog("Why is mouse pointing up?", 4, true, 4);
            Debug.Log("Why is the mouse point up??");
            return new Vector3();
        }

        float   rayLength = (mouseRay.origin.y / mouseRay.direction.y);
        Vector3 hitPos    = mouseRay.origin - (mouseRay.direction * rayLength);
        return hitPos;
    }

    public void OnMovement(InputAction.CallbackContext ctx) {

        // Debug.Log(string.Format("{0}", ctx.ReadValueAsObject()));
        movement = ctx.ReadValue<Vector2>();
        direction = new Vector3(movement.x, 0, movement.y);

        // Debug.Log(string.Format("{0}", movement));

        if (ctx.started && !ctx.performed) {
            //Set move started
            moveStarted = true;
        }

        if (ctx.canceled) {
            moveStarted = false;
        }

        //This is basically only doing stuff when clicked
        // if (ctx.performed) {
        //     Move(movement);
        // }
    }


    public void OnZoom(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            // Debug.Log(string.Format("Scroll Wheel {0}", ctx.ReadValue<Vector2>() ));
            ZoomCamera(ctx.ReadValue<Vector2>().y);
        }
    }


    /// <summary>
    /// This will return us the position of our input device as it moves
    ///
    /// TODO support XBOX controller with this as it's not supported now
    /// </summary>
    /// <param name="context"></param>
    public void OnPosition(InputAction.CallbackContext context) {
        mouse = context.ReadValue<Vector2>();
        mousePos = new Vector3(mouse.x, mouse.y, 0);

        if (context.started) {
            // Debug.Log(string.Format("OnPosition: {0}", context.ReadValue<Vector2>()));
        }
    }

    public void OnDrag(InputAction.CallbackContext ctx) {

        if (ctx.started) {
            if (ctx.interaction is SlowTapInteraction) {
                // Debug.Log(string.Format("Drag Start"));
                isDrag = true;
            }
        }

        // Commented out because we don't care about Clicks in OnDrag!
        // if (ctx.performed) {
        //     if (ctx.interaction is TapInteraction) {
        //         Debug.Log(string.Format("Click"));
        //     }
        // }

        if ((ctx.performed && ctx.interaction is SlowTapInteraction) || ctx.canceled) {
            // Debug.Log(string.Format("Dragging Stopped!!"));
            // Debug.Log(string.Format("Drag Stop"));
            isDrag = false;
        }


        // if (ctx.started) {
        //     if (ctx.interaction is SlowTapInteraction) {
        //
        //     }
        //     else {
        //         Debug.Log(string.Format("CLICK"));
        //     }
        //
        //     if (ctx.started && !ctx.performed) {
        //         // Debug.Log(string.Format("Dragging Started!!"));
        //         isDrag = true;
        //     }
        // }

    }

    /// <summary>
    /// Responsible for moving the player around for drawing
    /// </summary>
    /// <param name="direction"></param>
    public void Move(Vector2 direction) {
        var position = GameManager.instance.player.transform.position;
        var horizontal = direction.x;
        var vertical   = direction.y;

        var scaledMoveSpeed = GameManager.instance.moveSpeed * Time.deltaTime;
        var move = transform.TransformDirection(horizontal, vertical, 0);

        //Move the player just for giggles
        position.x += horizontal;
        position.y += vertical;

        // player.transform.localPosition += move * scaledMoveSpeed;
        GameManager.instance.player.transform.position += move * scaledMoveSpeed;
        // player.transform.position =

        // * GameManager.instance.moveSpeed
        // player.transform.position = direction;
        // Debug.Log(string.Format("We're moving: {0}", direction));

    }

}
