from transformers import AutoModelForCausalLM, AutoTokenizer

# Load the DialoGPT model
tokenizer = AutoTokenizer.from_pretrained("microsoft/DialoGPT-medium")
model = AutoModelForCausalLM.from_pretrained("microsoft/DialoGPT-medium")

# Test the model with a simple input
input_text = "Tell me a joke!"
input_ids = tokenizer.encode(input_text, return_tensors="pt")
output = model.generate(input_ids, max_length=50, pad_token_id=tokenizer.eos_token_id)

print(tokenizer.decode(output[0], skip_special_tokens=True))
