import requests
import random
import time
from datetime import datetime

while(True):
    agora = datetime.now()
    data_formatada = agora.strftime("%m/%d/%Y %H:%M:%S")

    temperature = random.randint(15, 30)  
    vibration = random.randint(10, 20)      
    current = random.randint(1, 5)  

    dados = {
        "date": data_formatada,
        "temperature": temperature,
        "vibration": vibration,
        "current": current
    }

    url = "http://localhost:5074/api/Sensors"

    response = requests.post(url, json=dados)  

    if response.status_code == 200 or response.status_code == 201:
        print("Post realizado com sucesso!")
        print("Resposta da API:", response.json())
    else:
        print(f"Falha ao fazer o POST. Status code: {response.status_code}")
        print("Resposta:", response.text)

    time.sleep(3000)
