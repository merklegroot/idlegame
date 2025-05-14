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
        payload = {
            "model": "llava",
            "prompt": "What is shown in this image? Describe the item in a single sentence, focusing on what it represents in a fantasy game context.",
            "stream": False,
            "images": [image_base64]
        }
        
        # Make the API call
        response = requests.post(url, json=payload)
        if response.status_code == 200:
            result = response.json()
            return result.get('response', 'No description available')
        else:
            return f"Error: HTTP {response.status_code}"
            
    except Exception as e:
        return f"Error: {str(e)}"

def main():
    # Get the current directory
    current_dir = Path(__file__).parent
    
    # Initialize the descriptions dictionary
    descriptions = {"descriptions": {}}
    
    # Process first three PNG files
    png_files = sorted([f for f in os.listdir(current_dir) if f.endswith('.png')])[:3]
    
    print("Processing images...")
    for png_file in png_files:
        print(f"Processing {png_file}...")
        image_path = current_dir / png_file
        description = get_image_description(image_path)
        descriptions["descriptions"][png_file] = description
        print(f"Got description: {description}")
    
    # Save descriptions to JSON file
    output_path = current_dir / "descriptions.json"
    with open(output_path, 'w', encoding='utf-8') as f:
        json.dump(descriptions, f, indent=2, ensure_ascii=False)
    
    print(f"\nDescriptions saved to {output_path}")

if __name__ == "__main__":
    main() 