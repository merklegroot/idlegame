#!/bin/bash

# Create a JSON file to store descriptions
output_file="descriptions.json"
echo "{" > "$output_file"
echo "  \"descriptions\": {" >> "$output_file"

# Process first three PNG files
count=0
for file in decor_0.png decor_1.png decor_2.png; do
    if [ -f "$file" ]; then
        # Get description from llava
        description=$(curl -s -X POST http://localhost:11434/api/generate -d "{
          \"model\": \"llava\",
          \"prompt\": \"What is shown in this image? Describe the item in a single sentence, focusing on what it represents in a fantasy game context.\",
          \"stream\": false,
          \"images\": [\"$(base64 "$file")\"]
        }" | jq -r '.response')
        
        # Add to JSON (with comma for all but last entry)
        if [ $count -lt 2 ]; then
            echo "    \"$file\": \"$description\"," >> "$output_file"
        else
            echo "    \"$file\": \"$description\"" >> "$output_file"
        fi
        
        count=$((count + 1))
    fi
done

# Close JSON structure
echo "  }" >> "$output_file"
echo "}" >> "$output_file" 