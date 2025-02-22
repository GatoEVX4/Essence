import os

def obter_tamanho_arquivos(diretorio_raiz):
    print("Calculando tempo necessário...")
    arquivos_tamanho = []
    for subdir, _, arquivos in os.walk(diretorio_raiz):
        for arquivo in arquivos:
            caminho_arquivo = os.path.join(subdir, arquivo)
            try:
                tamanho = os.path.getsize(caminho_arquivo)
                arquivos_tamanho.append((caminho_arquivo, tamanho))
            except (OSError, PermissionError):
                # Ignora arquivos que não podem ser acessados
                pass
    print("OK...")
    return sorted(arquivos_tamanho, key=lambda x: x[1])

def buscar_string_em_arquivos(diretorio_raiz, string_procurada):
    arquivos_encontrados = []
    arquivos_tamanho = obter_tamanho_arquivos(diretorio_raiz)
    total_arquivos = len(arquivos_tamanho)
    total_ocorrencias = 0

    for i, (caminho_arquivo, _) in enumerate(arquivos_tamanho):
        try:
            with open(caminho_arquivo, 'r', encoding="utf-8") as f:
                conteudo = f.read()
                if string_procurada in conteudo:
                    total_ocorrencias += conteudo.count(string_procurada)
                    arquivos_encontrados.append(caminho_arquivo)
                    print(f"String encontrada no arquivo: {caminho_arquivo}")
                    print("------------------------------------------------------------------------")
        except (UnicodeDecodeError, PermissionError):
            print(caminho_arquivo)
            # Ignora arquivos que não podem ser lidos
            pass

        progresso = (i + 1) / total_arquivos * 100
        print(f"Progresso: {progresso:.2f}%")

    return arquivos_encontrados, total_arquivos, total_ocorrencias

# Exemplo de uso
diretorio_raiz = './'  # Substitua pelo diretório desejado
string_procurada = input("String: ")  # Substitua pela string que deseja procurar

resultados, total_arquivos_lidos, total_ocorrencias = buscar_string_em_arquivos(diretorio_raiz, string_procurada)

if resultados:
    print("A string foi encontrada nos seguintes arquivos:")
    for resultado in resultados:
        print(resultado)
else:
    print("A string não foi encontrada em nenhum arquivo.")

print(f"Total de arquivos lidos: {total_arquivos_lidos}")
print(f"Total de ocorrências da string: {total_ocorrencias}")
print(f"Total de arquivos onde a string foi encontrada: {len(resultados)}")
input()
