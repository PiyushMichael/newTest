import React from 'react';
import ReactDOM from 'react-dom';

const strElement = <div><h1>Hello React! H1 element added</h1> <li>peugeot</li> <li>citreon</li> <li>renault</li></div>

class Garage extends React.Component
{
	render()
	{
		return <h1>this is the garage class inside car class</h1>;
	}
}

class Car extends React.Component
{
	constructor()
	{
		super();
		this.state = {bleh: "red", col: "EHEH"};
		this.innerHTML = "inner HTML text :O"
	}
	render()
	{
		return (
			<div>
				<Garage />
				<p>this paragraph of {this.state.bleh} car eheh</p>
				<p>second para {this.innerHTML}</p>
				<p>passing car details as a prop: {this.props.col.brand}, {this.props.col.model}</p>
				<p>same variable name as prop but in state {this.state.col}</p>
			</div>
		);
	}
}
class App extends React.Component
{
	
	render()
	{
		const cars = {brand: "Ford", model: 1998};
		return(ReactDOM.render(<Car col={cars}/>,document.getElementById('root')))
	}
}

export default App;

