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

def buscar_e_substituir_string_em_arquivos(diretorio_raiz, string_procurada, string_substituta):
    arquivos_encontrados = []
    arquivos_tamanho = obter_tamanho_arquivos(diretorio_raiz)
    total_arquivos = len(arquivos_tamanho)
    total_substituicoes = 0

    for i, (caminho_arquivo, _) in enumerate(arquivos_tamanho):
        try:
            with open(caminho_arquivo, 'r', encoding='utf-8') as f:
                conteudo = f.read()
                if string_procurada in conteudo:
                    conteudo_modificado = conteudo.replace(string_procurada, string_substituta)
                    total_substituicoes += conteudo.count(string_procurada)
                    with open(caminho_arquivo, 'w', encoding='utf-8') as f_modificado:
                        f_modificado.write(conteudo_modificado)
                    arquivos_encontrados.append(caminho_arquivo)
                    print(f"String encontrada e substituída no arquivo: {caminho_arquivo}")
                    print("------------------------------------------------------------------------")
        except (UnicodeDecodeError, PermissionError):
            # Ignora arquivos que não podem ser lidos
            pass

        progresso = (i + 1) / total_arquivos * 100
        print(f"Progresso: {progresso:.2f}%")

    return arquivos_encontrados, total_arquivos, total_substituicoes

# Exemplo de uso
diretorio_raiz = 'E:/IAN/Essence/Essence'  # Substitua pelo diretório desejado
string_procurada = input("String a procurar: ")  # Substitua pela string que deseja procurar
string_substituta = "Essence"  # String que substituirá a string procurada

resultados, total_arquivos_lidos, total_substituicoes = buscar_e_substituir_string_em_arquivos(diretorio_raiz, string_procurada, string_substituta)

if resultados:
    print("A string foi encontrada e substituída nos seguintes arquivos:")
    for resultado in resultados:
        print(resultado)
else:
    print("A string não foi encontrada em nenhum arquivo.")

print(f"Total de arquivos lidos: {total_arquivos_lidos}")
print(f"Total de strings substituídas: {total_substituicoes}")
print(f"Total de arquivos onde a substituição ocorreu: {len(resultados)}")
input()
