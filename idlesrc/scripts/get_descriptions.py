import os
import json
import base64
import requests
from pathlib import Path

def get_image_description(image_path):
    """Get description for a single image using llava model."""
    try:
        # Read and encode the image
        with open(image_path, 'rb') as img_file:
            image_base64 = base64.b64encode(img_file.read()).decode('utf-8')
        
        # Prepare the API request
        url = "http://localhost:11434/api/generate"
        prompt = "This is a pixel art image from a fantasy game tileset. It likely represents ground cover, crops, potted plants, stones, or logs. What specific outdoor decoration or natural element is shown in this image? Describe it in a single concise sentence."
        model = "llava"
        
        payload = {
            "model": model,
            "prompt": prompt,
            "stream": False,
            "images": [image_base64]
        }
        
        # Make the API call
        response = requests.post(url, json=payload)
        if response.status_code == 200:
            result = response.json()
            return {
                "fileName": image_path.name,
                "model": model,
                "prompt": prompt,
                "description": result.get('response', 'No description available').strip()
            }
        else:
            return {
                "fileName": image_path.name,
                "model": model,
                "prompt": prompt,
                "description": f"Error: HTTP {response.status_code}"
            }
            
    except Exception as e:
        return {
            "fileName": image_path.name,
            "model": model,
            "prompt": prompt,
            "description": f"Error: {str(e)}"
        }

def load_or_create_data(output_path):
    """Load existing data or create new data structure."""
    if output_path.exists():
        with open(output_path, 'r', encoding='utf-8') as f:
            return json.load(f)
    return []

def save_data(data, output_path):
    """Save data to JSON file."""
    output_path.parent.mkdir(parents=True, exist_ok=True)
    with open(output_path, 'w', encoding='utf-8') as f:
        json.dump(data, f, indent=2, ensure_ascii=False)

def should_process_file(filename, data, current_model, current_prompt):
    """Check if we should process this file based on existing entries."""
    for entry in data:
        if (entry["fileName"] == filename and 
            entry["model"] == current_model and 
            entry["prompt"] == current_prompt):
            return False
    return True

def main():
    # Get the project root directory (parent of scripts directory)
    root_dir = Path(__file__).parent.parent
    
    # Define paths
    images_dir = root_dir / "assets" / "cute-fantasy" / "individual_tiles" / "outdoor_decor"
    output_path = root_dir / "data" / "descriptions.json"
    
    # Debug: Print paths to verify
    print(f"Script location: {Path(__file__)}")
    print(f"Project root: {root_dir}")
    print(f"Images directory: {images_dir}")
    print(f"Output path: {output_path}")
    print(f"Images directory exists: {images_dir.exists()}")
    print(f"Output directory exists: {output_path.parent.exists()}")
    
    # Load or create data structure
    data = load_or_create_data(output_path)
    current_model = "llava"
    current_prompt = "This is a pixel art image from a fantasy game tileset. It likely represents ground cover, crops, potted plants, stones, or logs. What specific outdoor decoration or natural element is shown in this image? Describe it in a single concise sentence."
    
    # Process PNG files that haven't been processed yet
    png_files = sorted([f for f in os.listdir(images_dir) if f.endswith('.png')])
    files_to_process = [f for f in png_files if should_process_file(f, data, current_model, current_prompt)][:3]
    
    if not files_to_process:
        print("No new files to process!")
        return
    
    print("\nProcessing images...")
    for png_file in files_to_process:
        print(f"Processing {png_file}...")
        image_path = images_dir / png_file
        result = get_image_description(image_path)
        data.append(result)
        print(f"Got description: {result['description']}")
        
        # Save after each file is processed
        save_data(data, output_path)
        print(f"Updated descriptions saved to {output_path}")

if __name__ == "__main__":
    main() 