import yaml


def read_yaml_file(path: str) -> dict:
    yaml_content = ''
    with open(path, "r") as reader:
        yaml_content = reader.read()
    return yaml.safe_load(yaml_content)

