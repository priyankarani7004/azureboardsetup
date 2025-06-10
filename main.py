import easyocr
import cv2
import matplotlib.pyplot as plt

# Load your image
image_path = 'handwritten.jpeg'

# Initialize the EasyOCR Reader
reader = easyocr.Reader(['en'])  # Add other languages if needed

# Perform OCR
results = reader.readtext(image_path)

# Print results
for (bbox, text, prob) in results:
    print(text)
