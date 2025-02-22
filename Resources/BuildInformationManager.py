from datetime import datetime


with open('BuildInfos.txt', 'r+') as arquivo:
    linhas = arquivo.readlines()

    primeira_linha = linhas[0].strip()
    partes = primeira_linha.split('+')

    arquivo.seek(0)
    arquivo.write(f"{int(partes[0]) + 1}+{datetime.now().strftime("%d/%m/%Y %H:%M:%S")}")
    print(f"NEW BUILD: [{int(partes[0]) + 1}+{datetime.now().strftime("%d/%m/%Y %H:%M:%S")}]")
    arquivo.truncate()
