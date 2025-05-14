extends Control

@onready var progress_bar = $HBoxContainer/ProgressBar
@onready var wood_button = $HBoxContainer/WoodButton

var gathering = false
var gather_speed = 0.5  # Time in seconds to complete gathering
var progress = 0.0

func _ready():
	wood_button.pressed.connect(_on_wood_button_pressed)
	progress_bar.value = progress

func _process(delta):
	if gathering:
		progress += delta / gather_speed
		progress_bar.value = progress
		
		if progress >= 1.0:
			_on_gathering_complete()

func _on_wood_button_pressed():
	if not gathering:
		gathering = true
		progress = 0.0
		progress_bar.value = progress

func _on_gathering_complete():
	gathering = false
	progress = 0.0
	progress_bar.value = progress
	print("Wood gathered!") 