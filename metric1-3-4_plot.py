import requests
import matplotlib.pyplot as plt
import json
from matplotlib.ticker import MaxNLocator

# url to fetch data
# url = "https://portkey-2a1ae-default-rtdb.firebaseio.com/playtesting1_analytics.json"
url = "https://portkey-2a1ae-default-rtdb.firebaseio.com/beta_playtesting_analytics.json"


# fetch the data from the firebase 
def fetch_data(url):
    response = requests.get(url)
    data = response.json()
    return data

# fetch the data
data = fetch_data(url)

# parse and use the data
def parse_data(data, metric_left, metric_right):
    level_data = {}
    for player in data.values():
        level = int(player['level'])   #int
        value_left = player[metric_left]
        value_right = player[metric_right]
        
        if level not in level_data:
            level_data[level] = {'left': 0, 'right': 0, 'count': 0}
        
        level_data[level]['left'] += value_left
        level_data[level]['right'] += value_right
        level_data[level]['count'] += 1

    # calculate the averages
    levels = sorted(level_data.keys())

    average_left = [level_data[level]['left'] / level_data[level]['count'] for level in levels]
    average_right = [level_data[level]['right'] / level_data[level]['count'] for level in levels]


    return levels, average_left, average_right


# plot
def plot(levels, average_left, average_right, xlabel, ylabel, title, left_label, right_label):
    fig, ax = plt.subplots()
    bar_width = 0.5
    # stack
    p1 = ax.bar(levels, average_left, bar_width, label=left_label)
    p2 = ax.bar(levels, average_right, bar_width, bottom=average_left, label=right_label)

    # labels and titles for axis and graph
    ax.set_xlabel(xlabel)
    ax.set_ylabel(ylabel)
    ax.set_title(title)
    ax.set_xticks(levels)
    ax.legend()

    ax.yaxis.set_major_locator(MaxNLocator(integer=True)) #int

    # text annotations
    for level, left, right in zip(levels, average_left, average_right):
        if left > 0:
            ax.text(level, left / 2, f'{left:.2f}', ha='center', va='center', color='black')
        if right > 0:
            ax.text(level, left + right / 2, f'{right:.2f}', ha='center', va='center', color='black')
   
    plt.show()
    


# metric 2 - Level Completion Reason
# levels, collisions, time_ups = parse_data(data, 'reasonforFinshingLevel', 'reasonforFinshingLevel')
# plot(levels, collisions, time_ups, 'Levels', 'Total Number of Game Completions', 'Level Completion Reason', 'Collision', 'Time Up')

# metric 1 - Scores Collected per Level
levels, average_scores_left, average_scores_right = parse_data(data, 'scoreLeft', 'scoreRight')
plot(levels, average_scores_left, average_scores_right, 'Levels', 'Average ScoreUp Props Collected across Gameplays', 'ScoreUp Props Collected per Level', 'Left Screen', 'Right Screen')

# metric 3 - Usage of Control-Flipping Props
levels, average_props_left, average_props_right = parse_data(data, 'totalCtrlSwitchPropCollectedLeft', 'totalCtrlSwitchPropCollectedRight')
plot(levels, average_props_left, average_props_right, 'Levels', 'Average Control-Flipping Props across Gameplays', 'Usage of Control-Flipping Props', 'Left Screen', 'Right Screen')

# metric 4 - Collisions after Control-Flip per Level
levels, average_collisions_left, average_collisions_right = parse_data(data, 'collisionDueToCtrlFlipLeft', 'collisionDueToCtrlFlipRight')
plot(levels, average_collisions_left, average_collisions_right, 'Levels', 'Average Number of Obstacle Collisions across Gameplays', 'Collisions after Control-Flip per Level', 'Left Screen', 'Right Screen')