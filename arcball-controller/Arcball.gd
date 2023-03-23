extends Node3D


var enabled: bool

# Called when the node enters the scene tree for the first time.
func _ready():
	pass

func _input(event):
	if event is InputEventMouseButton:
		if event.button_index == MOUSE_BUTTON_LEFT:
			if event.pressed:
				enabled = true
			else:
				enabled = false

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if enabled:
		var mouse_screen_pos = get_viewport().get_mouse_position()
		var mouse_ndc = screen_to_ndc(mouse_screen_pos)
		var arcball = ndc_to_arcball(mouse_ndc)
		var center = Vector3(0.0, 0.0, 1.0)
		var rot_axis = arcball.cross(center).normalized()
		var angle = acos(clamp(arcball.dot(center), -1.0, 1.0))
		transform = Transform3D().rotated(rot_axis, angle)


func ndc_to_arcball(ndc: Vector2):
	var length = ndc.dot(ndc)
	if length > 1.0:
		return Vector3(ndc.x, ndc.y, 0.0).normalized()
	else:
		return Vector3(ndc.x, ndc.y, sqrt(1.0 - length)).normalized()

func screen_to_ndc(screen: Vector2):
	
	return Vector2(
		2.0 * screen.x / get_window().size.x - 1.0,
		1.0 - (2.0 * screen.y) / get_window().size.y
	)
